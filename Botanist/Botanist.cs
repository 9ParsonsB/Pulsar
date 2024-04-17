namespace Botanist;

using Observatory.Framework;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files.Journal.Exploration;
using Observatory.Framework.Files.Journal.Odyssey;
using Observatory.Framework.Files.Journal.Other;
using Observatory.Framework.Files.Journal.Startup;
using Observatory.Framework.Files.Journal.Travel;
using Observatory.Framework.Files.ParameterTypes;


public class Botanist : IObservatoryWorker
{
    private IObservatoryCore Core;
    private bool OdysseyLoaded;
    private Dictionary<BodyAddress, BioPlanetDetail> BioPlanets;

    // To make this journal locale agnostic, use the genus identifier and map to English names used in notifications.
    // Note: Values here are also used in the lookup for colony distance, so we also use this to resolve misspellings and Frontier bugs.
    public static readonly IReadOnlyDictionary<string, string> EnglishGenusByIdentifier = new Dictionary<string, string>
    {
        { "$Codex_Ent_Aleoids_Genus_Name;", "Aleoida" },
        { "$Codex_Ent_Bacterial_Genus_Name;", "Bacterium" },
        { "$Codex_Ent_Cactoid_Genus_Name;", "Cactoida" },
        {
            "$Codex_Ent_Clepeus_Genus_Name;;", "Clypeus"
        }, // Fun misspelling of the identifier discovered in the journals
        { "$Codex_Ent_Clypeus_Genus_Name;", "Clypeus" },
        { "$Codex_Ent_Conchas_Genus_Name;", "Concha" },
        { "$Codex_Ent_Electricae_Genus_Name;", "Electricae" },
        { "$Codex_Ent_Fonticulus_Genus_Name;", "Fonticulua" },
        { "$Codex_Ent_Shrubs_Genus_Name;", "Frutexa" },
        { "$Codex_Ent_Fumerolas_Genus_Name;", "Fumerola" },
        { "$Codex_Ent_Fungoids_Genus_Name;", "Fungoida" },
        { "$Codex_Ent_Osseus_Genus_Name;", "Osseus" },
        { "$Codex_Ent_Recepta_Genus_Name;", "Recepta" },
        { "$Codex_Ent_Stratum_Genus_Name;", "Stratum" },
        { "$Codex_Ent_Tubus_Genus_Name;", "Tubus" },
        { "$Codex_Ent_Tussocks_Genus_Name;", "Tussock" },
        { "$Codex_Ent_Ground_Struct_Ice_Name;", "Crystalline Shards" },
        { "$Codex_Ent_Brancae_Name;", "Brain Trees" },
        {
            "$Codex_Ent_Seed_Name;", "Brain Tree"
        }, // Misspelling? :shrug: 'Seed' also seems to refer to peduncle things.
        { "$Codex_Ent_Sphere_Name;", "Anemone" },
        { "$Codex_Ent_Tube_Name;", "Sinuous Tubers" },
        { "$Codex_Ent_Vents_Name;", "Amphora Plant" },
        { "$Codex_Ent_Cone_Name;", "Bark Mounds" },
    };

    // Note: Some Horizons bios may be missing, but they'll get localized genus name and default colony distance
    public static readonly IReadOnlyDictionary<string, int> ColonyDistancesByGenus = new Dictionary<string, int>()
    {
        { "Aleoida", 150 },
        { "Bacterium", 500 },
        { "Cactoida", 300 },
        { "Clypeus", 150 },
        { "Concha", 150 },
        { "Electricae", 1000 },
        { "Fonticulua", 500 },
        { "Frutexa", 150 },
        { "Fumerola", 100 },
        { "Fungoida", 300 },
        { "Osseus", 800 },
        { "Recepta", 150 },
        { "Stratum", 500 },
        { "Tubus", 800 },
        { "Tussock", 200 },
        { "Crystalline Shards", DEFAULT_COLONY_DISTANCE },
        { "Brain Tree", DEFAULT_COLONY_DISTANCE },
        { "Anemone", DEFAULT_COLONY_DISTANCE },
        { "Sinuous Tubers", DEFAULT_COLONY_DISTANCE },
        { "Amphora Plant", DEFAULT_COLONY_DISTANCE },
        { "Bark Mounds", DEFAULT_COLONY_DISTANCE },
    };

    private const int DEFAULT_COLONY_DISTANCE = 100;

    private Guid? samplerStatusNotification;

    private BotanistSettings botanistSettings = new()
    {
        OverlayEnabled = true,
        OverlayIsSticky = true,
    };

    public string Name => "Observatory Botanist";

    public string ShortName => "Botanist";

    public string Version => typeof(Botanist).Assembly.GetName().Version.ToString();

    public object Settings
    {
        get => botanistSettings;
        set => botanistSettings = (BotanistSettings)value;
    }

    public void JournalEvent<TJournal>(TJournal journal) where TJournal : JournalBase
    {
        switch (journal)
        {
            case LoadGame loadGame:
                OdysseyLoaded = loadGame.Odyssey;
                break;
            case SAASignalsFound signalsFound:
            {
                BodyAddress systemBodyId = new()
                {
                    SystemAddress = signalsFound.SystemAddress,
                    BodyID = signalsFound.BodyID
                };
                if (OdysseyLoaded && !BioPlanets.ContainsKey(systemBodyId))
                {
                    var bioSignals = from signal in signalsFound.Signals
                        where signal.Type == "$SAA_SignalType_Biological;"
                        select signal;

                    if (bioSignals.Any())
                    {
                        BioPlanets.Add(
                            systemBodyId,
                            new()
                            {
                                BodyName = signalsFound.BodyName,
                                BioTotal = bioSignals.First().Count,
                                SpeciesFound = new()
                            }
                        );
                    }
                }
            }
                break;
            case ScanOrganic scanOrganic:
            {
                BodyAddress systemBodyId = new()
                {
                    SystemAddress = scanOrganic.SystemAddress,
                    BodyID = scanOrganic.Body
                };
                if (!BioPlanets.ContainsKey(systemBodyId))
                {
                    // Unlikely to ever end up in here, but just in case create a new planet entry.
                    Dictionary<string, BioSampleDetail> bioSampleDetails = new();
                    bioSampleDetails.Add(scanOrganic.Species_Localised, new()
                    {
                        Genus = EnglishGenusByIdentifier.GetValueOrDefault(scanOrganic.Genus,
                            scanOrganic.Genus_Localised),
                        Analysed = false
                    });

                    BioPlanets.Add(systemBodyId, new()
                    {
                        BodyName = string.Empty,
                        BioTotal = 0,
                        SpeciesFound = bioSampleDetails
                    });
                }
                else
                {
                    var bioPlanet = BioPlanets[systemBodyId];

                    switch (scanOrganic.ScanType)
                    {
                        case ScanOrganicType.Log:
                        case ScanOrganicType.Sample:
                            if (!Core.IsLogMonitorBatchReading && botanistSettings.OverlayEnabled)
                            {
                                var colonyDistance = GetColonyDistance(scanOrganic);
                                var sampleNum = scanOrganic.ScanType == ScanOrganicType.Log ? 1 : 2;
                                NotificationArgs args = new()
                                {
                                    Title = scanOrganic.Species_Localised,
                                    Detail =
                                        $"Sample {sampleNum} of 3{Environment.NewLine}Colony distance: {colonyDistance} m",
                                    Rendering = NotificationRendering.NativeVisual,
                                    Timeout = (botanistSettings.OverlayIsSticky ? 0 : -1),
                                    Sender = ShortName,
                                };
                                if (samplerStatusNotification == null)
                                {
                                    var notificationId = Core.SendNotification(args);
                                    if (botanistSettings.OverlayIsSticky)
                                        samplerStatusNotification = notificationId;
                                }
                                else
                                {
                                    Core.UpdateNotification(samplerStatusNotification.Value, args);
                                }
                            }

                            if (!bioPlanet.SpeciesFound.ContainsKey(scanOrganic.Species_Localised))
                            {
                                bioPlanet.SpeciesFound.Add(scanOrganic.Species_Localised, new()
                                {
                                    Genus = EnglishGenusByIdentifier.GetValueOrDefault(scanOrganic.Genus,
                                        scanOrganic.Genus_Localised),
                                    Analysed = false
                                });
                            }

                            break;
                        case ScanOrganicType.Analyse:
                            if (!bioPlanet.SpeciesFound[scanOrganic.Species_Localised].Analysed)
                            {
                                bioPlanet.SpeciesFound[scanOrganic.Species_Localised].Analysed = true;
                            }

                            MaybeCloseSamplerStatusNotification();
                            break;
                    }
                }
            }
                break;
            case LeaveBody:
            case FSDJump:
            case Shutdown:
                // These are all good reasons to kill any open notification. Note that SupercruiseEntry is NOT a
                // suitable reason to close the notification as the player hopping out only to double check the
                // DSS map for another location. Note that a game client crash will not close the status notification.
                MaybeCloseSamplerStatusNotification();
                break;
        }
    }

    private object GetColonyDistance(ScanOrganic scan)
    {
        // Map the Genus to a Genus name then lookup colony distance.
        return ColonyDistancesByGenus.GetValueOrDefault(
            EnglishGenusByIdentifier.GetValueOrDefault(scan.Genus, string.Empty), DEFAULT_COLONY_DISTANCE);
    }

    private void MaybeCloseSamplerStatusNotification()
    {
        if (samplerStatusNotification != null)
        {
            Core.CancelNotification(samplerStatusNotification.Value);
            samplerStatusNotification = null;
        }
    }

    public void Load(IObservatoryCore observatoryCore)
    {
        BioPlanets = new();

        Core = observatoryCore;
    }
}