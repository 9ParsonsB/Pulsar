﻿namespace Observatory.Framework.Files.Journal.Other;

public class SendText : JournalBase
{
    public string To { get; init; }
    public string To_Localised { get; init; }
    public string Message { get; init; }
    public bool Sent { get; init; }
}