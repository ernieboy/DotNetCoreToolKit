namespace DotNetCoreToolKit.Library.Models.Persistence
{
    public class Enums
    {
        /// <summary>
        /// Enum that represents client-side entity state
        /// </summary>
        public enum ObjectState
        {
            Unchanged = 0,
            Added = 1,
            Modified = 2,
            Deleted = 3
        }
    }
}
