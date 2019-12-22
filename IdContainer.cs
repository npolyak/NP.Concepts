namespace NP.Concepts
{
    public class IdContainer
    {
        public int CurrentId { get; private set; } = 0;

        /// <summary>
        /// increase id by 1
        /// </summary>
        public void Increase()
        {
            CurrentId++;
        }

        /// <summary>
        /// set it to passed arg is it is bigger than the id
        /// </summary>
        /// <param name="id"></param>
        public void SetToIfSmaller(int id)
        {
            if (id > CurrentId)
            {
                CurrentId = id;
            }
        }

        /// <summary>
        /// reset id to 0
        /// </summary>
        public void Reset()
        {
            CurrentId = 0;
        }
    }
}
