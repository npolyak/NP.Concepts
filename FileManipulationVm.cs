using NP.Concepts.DatumAttributes;

namespace NP.Concepts
{
    public class FileManipulationVm : ProgressVm
    {
        #region FilePath Property
        [DatumProperty(DatumPropertyDirection.In, true)]
        public virtual string FilePath
        {
            get; set;
        }
        #endregion FilePath Property

    }
}
