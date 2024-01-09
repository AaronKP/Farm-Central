using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ST10090758_FarmCentral.Models;// import to access the User and Product class ( want to return list of Users and products)
using System.Data;
using System.Data.SqlClient;
using ST10090758_FarmCentral.Controllers;// to access the obj storing the logged in userID
using Microsoft.CodeAnalysis;
using System.Xml.Linq;

namespace ST10090758_FarmCentral.Database
{
    public class DBoperations
    {
        public static int myUserId;
        public static string myUserType;
        public static string myName;
        public static int myuserid4product;
        public static string myProdType;
       

        //properties for username and password so that we can work with this data across pages
        public string UserName { get; set; }
        public string Password { get; set; }
        //store user type to define roles
        public string UserType { get; set; }
        public int UserID { get; set; }


       

        //can use this to fetch con strings from the app.json file. 
        private IConfiguration _configuration;
        private string con;

        public DBoperations(IConfiguration configuration)
        {
            _configuration = configuration;
            //connection string in app.json
            this.con = _configuration.GetConnectionString("connectToDB");

        }

        //not used
        public List<User> getFarmerOnly(string userType)
        {
            List<User> userList = new List<User>();// list that stores user objects from the db

            SqlConnection myConnection = new SqlConnection(con);//store connection string/establish connection

            SqlCommand cmd = new SqlCommand($"select * from users where userType='{userType}'", myConnection);
            //will be used to convert queried results (of no type) from cmd into a data table
            SqlDataAdapter myAdpater = new SqlDataAdapter(cmd);

            DataTable myTable = new DataTable();//tables are an array of rows
            DataRow myRow;//for the table . rows are an array of cols. will store a row of the table

            //variables to store the data in each row
            string name = "", surname = "", type = "", userName = "", password = "";
            int userID = 0;

            myConnection.Open();
            //convert results into table myTable
            myAdpater.Fill(myTable);
            //data is now in the table after the above line of code.

            //store table in the list
            if (myTable.Rows.Count > 0)// checks if theres something in table
            {
                for (int i = 0; i < myTable.Rows.Count; i++)//run across entire row
                {
                    myRow = myTable.Rows[i];//work with 1 row per iteration

                    userID = (int)myRow[0];
                    name = (string)myRow[1];//stNum from db is located in col at index 0. Cast to string. covrt change data type of value, cast change of variable
                    surname = (string)myRow[2];// cols don't have a data type. Only the values in the cols have data types therefore cast
                    type = (string)myRow[3];
                    userName = (string)myRow[4];
                    password = (string)myRow[5];

                    //create a User object
                    User u = new User(userID, name, surname, type, userName, password);
                    userList.Add(u);//add object to list that will be returned
                }
            }
            myConnection.Close();//close connection
            return userList;
        }

        //method to retrireve user for login
        public void getUserLogin(string userName)//sign in method that retrieves login data from the db
        {
           
            string sqlSelect = $"SELECT * FROM users WHERE UserName = '{userName}'";//match col
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand cmdSelect = new SqlCommand(sqlSelect, myConnection);
                myConnection.Open();//open conection for reader
                using (SqlDataReader reader = cmdSelect.ExecuteReader())//to read data. ExecuteReader returns a data reader object wit the results from the query
                {
                    while (reader.Read())// while reading records from db
                    {
                        myUserId = Convert.ToInt32(reader[0]);
                        UserID = myUserId;
                        myName= (string)reader[1];
                        UserType = (string)reader[3];
                        myUserType=UserType.ToString();
                        //1st col of db
                        UserName = (string)reader[4];
                        //second col
                        Password = (string)reader[5];


                    }
                }
            }
        }

        //method to return ALL users from Farm Central DB
        public List<User> allUsers()
        {
            List<User> userList = new List<User>();// list that stores user objects from the db

            SqlConnection myConnection = new SqlConnection(con);//store connection string/establish connection

            SqlCommand cmd = new SqlCommand("select * from users", myConnection);
            //will be used to convert queried results (of no type) from cmd into a data table
            SqlDataAdapter myAdpater = new SqlDataAdapter(cmd);

            DataTable myTable = new DataTable();//tables are an array of rows
            DataRow myRow;//for the table . rows are an array of cols. will store a row of the table

            //variables to store the data in each row
            string name = "", surname = "", type="", userName="", password="";
            int userID = 0;

            myConnection.Open();
            //convert results into table myTable
            myAdpater.Fill(myTable);
            //data is now in the table after the above line of code.

            //store table in the list
            if (myTable.Rows.Count > 0)// checks if theres something in table
            {
                for (int i = 0; i < myTable.Rows.Count; i++)//run across entire row
                {
                    myRow = myTable.Rows[i];//work with 1 row per iteration

                    userID = (int)myRow[0];
                    name = (string)myRow[1];//stNum from db is located in col at index 0. Cast to string. covrt change data type of value, cast change of variable
                    surname = (string)myRow[2];// cols don't have a data type. Only the values in the cols have data types therefore cast
                    type = (string)myRow[3];
                    userName = (string)myRow[4];
                    password = (string)myRow[5];

                    //create a User object
                    User u = new User(userID,name,surname,type,userName,password);
                    userList.Add(u);//add object to list that will be returned
                }
            }
            myConnection.Close();//close connection
            return userList;
        }

        //method to set the userID
        public void setUserID()
        {
            SqlConnection myConnection = new SqlConnection(con);//store connection string/establish connection

            SqlCommand cmd = new SqlCommand($"select * from products", myConnection);
            //will be used to convert queried results (of no type) from cmd into a data table
            SqlDataAdapter myAdpater = new SqlDataAdapter(cmd);

            DataTable myTable = new DataTable();//tables are an array of rows
            DataRow myRow;//for the table . rows are an array of cols. will store a row of the table

            myConnection.Open();
            //convert results into table myTable
            myAdpater.Fill(myTable);
            //data is now in the table after the above line of code.

            //store table in the list
            if (myTable.Rows.Count > 0)// checks if theres something in table
            {
                for (int i = 0; i < myTable.Rows.Count; i++)//run across entire row
                {
                    myRow = myTable.Rows[i];//work with 1 row per iteration

                   
                    UserController.dbOp.UserID = (int)myRow[6];

                    
                }
            }
            myConnection.Close();//close connection
        }


        //method to return ALL products from db for a user
        public List<Product> allProducts(int id)
        {
             //setUserID();
        List<Product> productList = new List<Product>();// list that stores user objects from the db

        SqlConnection myConnection = new SqlConnection(con);//store connection string/establish connection

            SqlCommand cmd = new SqlCommand($"select * from products WHERE userID={id}", myConnection);
            //will be used to convert queried results (of no type) from cmd into a data table
            SqlDataAdapter myAdpater = new SqlDataAdapter(cmd);

            DataTable myTable = new DataTable();//tables are an array of rows
            DataRow myRow;//for the table . rows are an array of cols. will store a row of the table

            //variables to store the data in each row
            string name = "", description = "", type = "";
            int productID = 0, userID = 0;
            double price = 0.0;
            DateTime dateSupplied;

            myConnection.Open();
            //convert results into table myTable
            myAdpater.Fill(myTable);
            //data is now in the table after the above line of code.

            //store table in the list
            if (myTable.Rows.Count > 0)// checks if theres something in table
            {
                for (int i = 0; i < myTable.Rows.Count; i++)//run across entire row
                {
                    myRow = myTable.Rows[i];//work with 1 row per iteration

                    productID = (int)myRow["productID"];//productID from db is located in col at index 0. Cast to string. covrt change data type of value, cast change of variable
                    name = (string)myRow["name"];// cols don't have a data type. Only the values in the cols have data types therefore cast
                    type = (string)myRow["type"];
                    myProdType = type;
                    description = (string)myRow["description"];
                    price = Convert.ToDouble(myRow["price"]);
                    dateSupplied = (DateTime)myRow["dateSupplied"];
                    userID= (int)myRow["userID"];
                    myuserid4product = userID;
                    getProductOwner(productID);

                    //create a User object
                    Product product = new Product(productID,name,type,description,price,dateSupplied,userID);
                    productList.Add(product);//add object to list that will be returned
                }
            }
            myConnection.Close();//close connection
            return productList;
        }

        //filter product method
        public List<Product> filteredList(int userid,string prodtype)
        {
            List<Product> productList = new List<Product>();// list that stores user objects from the db

            SqlConnection myConnection = new SqlConnection(con);//store connection string/establish connection

            SqlCommand cmd = new SqlCommand($"select * from products WHERE userID={userid} AND type='{prodtype}'", myConnection);
            //will be used to convert queried results (of no type) from cmd into a data table
            SqlDataAdapter myAdpater = new SqlDataAdapter(cmd);

            DataTable myTable = new DataTable();//tables are an array of rows
            DataRow myRow;//for the table . rows are an array of cols. will store a row of the table

            //variables to store the data in each row
            string name = "", description = "", type = "";
            int productID = 0, userID = 0;
            double price = 0.0;
            DateTime dateSupplied;

            myConnection.Open();
            //convert results into table myTable
            myAdpater.Fill(myTable);
            //data is now in the table after the above line of code.

            //store table in the list
            if (myTable.Rows.Count > 0)// checks if theres something in table
            {
                for (int i = 0; i < myTable.Rows.Count; i++)//run across entire row
                {
                    myRow = myTable.Rows[i];//work with 1 row per iteration

                    productID = (int)myRow["productID"];//productID from db is located in col at index 0. Cast to string. covrt change data type of value, cast change of variable
                    name = (string)myRow["name"];// cols don't have a data type. Only the values in the cols have data types therefore cast
                    type = (string)myRow["type"];
                    description = (string)myRow["description"];
                    price = Convert.ToDouble(myRow["price"]);
                    dateSupplied = (DateTime)myRow["dateSupplied"];
                    userID = (int)myRow["userID"];
                    getProductOwner(productID);

                    //create a User object
                    Product product = new Product(productID, name, type, description, price, dateSupplied, userID);
                    productList.Add(product);//add object to list that will be returned
                }
            }
            myConnection.Close();//close connection
            return productList;
        }

        // method to get the FARMER'S name to display it on the Farmer products page
        public string getProductOwner(int prodID)
        {
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                //dont have to use adapter (that converst results into a table) because we only want 1 record
                SqlCommand cmdSelect = new SqlCommand($"SELECT u.name FROM products p JOIN users u ON p.userID=u.userID WHERE productID= '{prodID}'", myConnection);
                myConnection.Open();
                //Use Sql data reader (executes commands that you specify) instead of data table for single records
                using (SqlDataReader reader = cmdSelect.ExecuteReader())//executes select command that was specified
                {
                    while (reader.Read())
                    {
                        myName = (string)reader["name"];
                    }
                }
            }// as soon as code reaches this brace, connection closes
            return myName;
        }
        public int getProductOwnerID(int prodID)
        {
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                //dont have to use adapter (that converst results into a table) because we only want 1 record
                SqlCommand cmdSelect = new SqlCommand($"SELECT userID FROM products WHERE productID={prodID} ", myConnection);
                myConnection.Open();
                //Use Sql data reader (executes commands that you specify) instead of data table for single records
                using (SqlDataReader reader = cmdSelect.ExecuteReader())//executes select command that was specified
                {
                    while (reader.Read())
                    {
                        myuserid4product = (int)reader["userID"];
                    }
                }
            }// as soon as code reaches this brace, connection closes
            return myuserid4product;
        }
        //method to CREATE /add a new User
        public void addUser(User u)// since we are outside the user class we need the user object as a reference for values to insert into db
        {
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand cmdInsert = new SqlCommand($"INSERT INTO users VALUES('{u.Name}','{u.Surname}','{u.UserType}','{u.UserName}','{u.Password}')", myConnection);
                //non query statements, create/insert, delete, update
                //query statements, select

                SqlTransaction myTransaction;

                myConnection.Open();

                //rollback and commit error handling . Used for delete update and insert
                myTransaction = myConnection.BeginTransaction();//available as long as connection is open
                cmdInsert.Transaction = myTransaction;
                try
                {
                    cmdInsert.ExecuteNonQuery();//returns an integer
                    myTransaction.Commit();
                }
                catch (Exception)
                {
                    myTransaction.Rollback();
                }

            }
        }

        //method to CREATE/add a new product
        public void addProduct(Product prod)// since we are outside the user class we need the user object as a reference for values to insert into db
        {
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand cmdInsert = new SqlCommand($"INSERT INTO products VALUES('{prod.ProductName}','{prod.Type}','{prod.Description}',{prod.Price},'{prod.SupplyDate}',{prod.UserID})", myConnection);
                //non query statements, create/insert, delete, update
                //query statements, select

                SqlTransaction myTransaction;

                myConnection.Open();

                //rollback and commit error handling . Used for delete update and insert
                myTransaction = myConnection.BeginTransaction();//available as long as connection is open
                cmdInsert.Transaction = myTransaction;
                try
                {
                    cmdInsert.ExecuteNonQuery();//returns an integer
                    myTransaction.Commit();
                }
                catch (Exception)
                {
                    myTransaction.Rollback();
                }

            }
        }

        //method to return a specific User
        public User getUserDetails(int id)
        {
            string name, surname, userType,userName,password;//object will be built based on these attributes
            int userID;
            User userObj = new User();
            //alternative way of using open and close
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                //dont have to use adapter (that converst results into a table) because we only want 1 record
                SqlCommand cmdSelect = new SqlCommand($"SELECT * FROM users WHERE userID= '{id}'", myConnection);
                myConnection.Open();
                //Use Sql data reader (executes commands that you specify) instead of data table, for single records
                using (SqlDataReader reader = cmdSelect.ExecuteReader())//executes select command that was specified
                {
                    while (reader.Read())
                    {
                        userID = (int)reader[0];//colums don't have tyopes therefore cast to approriate type
                        name = (string)reader[1];
                        surname = (string)reader[2];
                        userType = (string)reader[3];
                        userName = (string)reader[4];
                        password = (string)reader[5];

                        userObj = new User(userID, name, surname,userType,userName,password);
                    }
                }
            }// as soon as code reaches this brace, connection closes
            return userObj;
        }

        //method to return a specific product
        public Product getProductDetails(int id)
        {
            string name, type, description;//object will be built based on these attributes
            int productID, userID;
            double price;
            DateTime dateSupplied;
            Product prodObj = new Product();
            //alternative way of using open and close
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                //dont have to use adapter (that converst results into a table) because we only want 1 record
                SqlCommand cmdSelect = new SqlCommand($"SELECT * FROM products WHERE productID= '{id}'", myConnection);
                myConnection.Open();
                //Use Sql data reader (executes commands that you specify) instead of data table for single records
                using (SqlDataReader reader = cmdSelect.ExecuteReader())//executes select command that was specified
                {
                    while (reader.Read())
                    {
                        productID = (int)reader[0];//colums don't have tyopes therefore cast to approriate type
                        name = (string)reader[1];
                        type = (string)reader[2];
                        description = (string)reader[3];
                        price = Convert.ToDouble(reader[4]);
                        dateSupplied = (DateTime)reader[5];
                        userID = (int)reader[6];

                        prodObj = new Product(productID,name,type,description,price,dateSupplied,userID);
                    }
                }
            }// as soon as code reaches this brace, connection closes
            return prodObj;
        }

        //method to delete a specific user
        public void deleteUser(int id)
        {
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand cmdDelete = new SqlCommand($"DELETE FROM users WHERE userID={id}", myConnection);
                //non query statements, create/insert, delete, update
                //query statements, select

                SqlTransaction myTransaction;

                myConnection.Open();

                //rollback and commit error handling . Used for delete update and insert
                myTransaction = myConnection.BeginTransaction();//available as long as connection is open
                cmdDelete.Transaction = myTransaction;
                try
                {
                    cmdDelete.ExecuteNonQuery();//returns an integer
                    myTransaction.Commit();
                }
                catch (Exception)
                {
                    myTransaction.Rollback();
                }

            }
        }


        //method to delete a specific product
        public void deleteProduct(int id)
        {
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand cmdDelete = new SqlCommand($"DELETE FROM products WHERE productID={id}", myConnection);
                //non query statements, create/insert, delete, update
                //query statements, select

                SqlTransaction myTransaction;

                myConnection.Open();

                //rollback and commit error handling . Used for delete update and insert
                myTransaction = myConnection.BeginTransaction();//available as long as connection is open
                cmdDelete.Transaction = myTransaction;
                try
                {
                    cmdDelete.ExecuteNonQuery();//returns an integer
                    myTransaction.Commit();
                }
                catch (Exception)
                {
                    myTransaction.Rollback();
                }

            }
        }

        //method to edit a specific user
        public void editUser(int id, string name, string surname,string type, string username,string password)
        {
            User u = new User();
            u.Name = name;
            u.Surname = surname;
            u.UserType = type;
            u.UserName=username;
            u.Password = password;
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand cmdInsert = new SqlCommand($"UPDATE users SET name='{u.Name}', surname ='{u.Surname}', userType='{u.UserType}', userName='{u.UserName}', password='{u.Password}' WHERE userID={id}", myConnection);
                //non query statements, create/insert, delete, update
                //query statements, select

                SqlTransaction myTransaction;

                myConnection.Open();

                //rollback and commit error handling . Used for delete update and insert
                myTransaction = myConnection.BeginTransaction();//available as long as connection is open
                cmdInsert.Transaction = myTransaction;
                try
                {
                    cmdInsert.ExecuteNonQuery();//returns an integer
                    myTransaction.Commit();
                }
                catch (Exception)
                {
                    myTransaction.Rollback();
                }

            }
           
        }


        //method to edit a specific user
        public void editProduct(int id, string name, string type, string description, double price, DateTime dateSupplied)
        {
            Product prod = new Product();
            prod.ProductName = name;
            prod.Type = type;
            prod.Description = description;
            prod.Price = price;
            prod.SupplyDate = dateSupplied;
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand cmdInsert = new SqlCommand($"UPDATE products SET name='{prod.ProductName}', type ='{prod.Type}', description='{prod.Description}', price={prod.Price}, dateSupplied='{prod.SupplyDate}' WHERE productID={id}", myConnection);
                //non query statements, create/insert, delete, update
                //query statements, select

                SqlTransaction myTransaction;

                myConnection.Open();

                //rollback and commit error handling . Used for delete update and insert
                myTransaction = myConnection.BeginTransaction();//available as long as connection is open
                cmdInsert.Transaction = myTransaction;
                try
                {
                    cmdInsert.ExecuteNonQuery();//returns an integer
                    myTransaction.Commit();
                }
                catch (Exception)
                {
                    myTransaction.Rollback();
                }

            }

        }
    }
}
