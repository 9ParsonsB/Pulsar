﻿namespace Observatory.Framework.Files.Journal
{
    public class CrewMemberRoleChange : CrewMemberJoins
    {
        public string Role { get; init; }
        public bool Telepresence { get; init; }
    }
}
