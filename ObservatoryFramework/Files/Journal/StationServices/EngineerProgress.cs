using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Framework.Files.Journal.StationServices;

public class EngineerProgress : JournalBase
{
    public override string Event => "EngineerProgress";
    public string? Engineer { get; set; }
    public ulong? EngineerID { get; set; }
    public int? Rank { get; set; }
    public int? RankProgress { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Progress? Progress { get; set; }

    public List<EngineerType> Engineers { get; set; }
}
//{ "timestamp":"2024-05-25T04:44:34Z", "event":"EngineerProgress",
//"Engineers":[
//{ "Engineer":"Hera Tani", "EngineerID":300090, "Progress":"Known" },
//{ "Engineer":"Professor Palin", "EngineerID":300220, "Progress":"Invited" },
//{ "Engineer":"Felicity Farseer", "EngineerID":300100, "Progress":"Unlocked", "RankProgress":0, "Rank":5 },
//{ "Engineer":"Eleanor Bresa", "EngineerID":400011, "Progress":"Known" },
//{ "Engineer":"Hero Ferrari", "EngineerID":400003, "Progress":"Known" },
//{ "Engineer":"Jude Navarro", "EngineerID":400001, "Progress":"Known" },
//{ "Engineer":"Etienne Dorn", "EngineerID":300290, "Progress":"Unlocked", "RankProgress":0, "Rank":5 },
//{ "Engineer":"Lori Jameson", "EngineerID":300230, "Progress":"Known" },
//{ "Engineer":"Liz Ryder", "EngineerID":300080, "Progress":"Unlocked", "RankProgress":86, "Rank":3 },
//{ "Engineer":"Rosa Dayette", "EngineerID":400012, "Progress":"Known" },
//{ "Engineer":"Juri Ishmaak", "EngineerID":300250, "Progress":"Unlocked", "RankProgress":0, "Rank":1 },
//{ "Engineer":"Zacariah Nemo", "EngineerID":300050, "Progress":"Known" },
//{ "Engineer":"Mel Brandon", "EngineerID":300280, "Progress":"Known" },
//{ "Engineer":"Selene Jean", "EngineerID":300210, "Progress":"Unlocked", "RankProgress":11, "Rank":3 },
//{ "Engineer":"Marco Qwent", "EngineerID":300200, "Progress":"Unlocked", "RankProgress":27, "Rank":4 },
//{ "Engineer":"Chloe Sedesi", "EngineerID":300300, "Progress":"Invited" },
//{ "Engineer":"Baltanos", "EngineerID":400010, "Progress":"Known" },
//{ "Engineer":"Petra Olmanova", "EngineerID":300130, "Progress":"Unlocked", "RankProgress":0, "Rank":5 },
//{ "Engineer":"The Dweller", "EngineerID":300180, "Progress":"Unlocked", "RankProgress":0, "Rank":1 },
//{ "Engineer":"Elvira Martuuk", "EngineerID":300160, "Progress":"Unlocked", "RankProgress":52, "Rank":3 },
//{ "Engineer":"Tod 'The Blaster' McQuinn", "EngineerID":300260, "Progress":"Unlocked", "RankProgress":15, "Rank":4 },
//{ "Engineer":"Domino Green", "EngineerID":400002, "Progress":"Invited" } ] }
// 