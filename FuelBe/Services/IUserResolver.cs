namespace FuelBe.Services {
    public interface IUserResolver {
        public bool IsAdmin { get; }
        public bool IsLogged { get; }
        public int Id { get; }
        void setId(int id);
        int getId();
    }
}
