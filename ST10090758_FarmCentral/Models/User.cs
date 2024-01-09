namespace ST10090758_FarmCentral.Models
{
    public class User
    {
        //properties for a user
        public int UserID { get; set; }
        public string Name{ get; set; }
        public string Surname { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        //user constructr
        public User(int userID, string name, string surname, string userType, string userName, string password)
        {
            UserID = userID;
            Name = name;
            Surname = surname;
            UserType = userType;
            UserName = userName;
            Password = password;
        }

        //constructor for adding Users to db
        public User(string name, string surname, string userType, string userName, string password) 
        {
            Name = name;
            Surname= surname;
            UserType = userType;
            UserName = userName;
            Password = password;
        }

        


        //login constructor
        public User(string userName, string pass)
        {
            UserName = userName;
            Password = pass;
        }

        public User()
        {

        }
    }

}
