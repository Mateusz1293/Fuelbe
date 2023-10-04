namespace FuelBe.Services {
    public class UserResolver : IUserResolver {
        private int Id;

        public void setId(int id)
        {
            this.Id = id;
        }

        public int getId()
        {
            return this.Id;
        }
    }
}
