using System.ComponentModel;

namespace CommunicationLibrary.Enums
{
    public enum PartiotionKeys
    {
        [Description("One")]
        One = 1,
        [Description("Two")]
        Two,
        [Description("Three")]
        Three,
        [Description("Four")]
        Four,
        [Description("Five")]
        Five
    }

    public enum BankMembership
    {
        [Description("Bronze")]
        Bronze = 1,
        [Description("Silver")]
        Silver,
        [Description("Gold")]
        Gold,
        [Description("Platinum")]
        Platinum
    }
}
