namespace NP.Concepts.DatumAttributes
{
    public class MethodToTurnToDatumProcessorAttribute : ComponentMetadataAttributeBase
    {
        public MethodToTurnToDatumProcessorAttribute
        (
            string icon = null,
            string displayName = null,
            string description = null
        ) :
            base(icon, displayName, description)
        {

        }
    }
}
