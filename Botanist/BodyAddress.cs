namespace Botanist;

class BodyAddress
{
    public ulong SystemAddress { get; set; }
    public int BodyID { get; set; }

    public override bool Equals(object obj)
    {
        // We want value equality here.

        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (BodyAddress)obj;
        return other.SystemAddress == SystemAddress
               && other.BodyID == BodyID;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SystemAddress, BodyID);
    }
}