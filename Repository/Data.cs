using Car_System.Models;
using System.Data.SqlClient;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Hosting;

namespace Car_System.Repository
{
    public class Data : IData
    {
        private readonly IConfiguration configuration;
        private readonly string dbcon = "";
        private readonly IWebHostEnvironment webhost;
        public Data(IConfiguration configuration, IWebHostEnvironment webHost) 
        {
            this.configuration = configuration;
            dbcon = this.configuration.GetConnectionString("dbConnection");
            this.webhost = webHost;
        }


        [SupportedOSPlatform("windows")]
        public DriverHistory GetDriverHistory(int Id)
        {
            DriverHistory hist = new DriverHistory();
            Driver dr = new Driver();
            Rent rent;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = String.Format("Select r.ID,r.PickUp,r.DropOff,r.PickUpDate,r.DropOffDate,r.TotalRun,r.Brand,r.Model,d.DriverName,d.Address,d.MobileNo,d.Experince,d.ImagePath " +
                    "from Rents r inner join Drivers d on r.DriverId = d.ID where d.ID = {0};",Id);
                SqlDataReader reader = GetData(qry, con);
                if(!reader.HasRows)
                {
                    hist.Driver=dr;
                }
                int i = 0;
                while (reader.Read())
                {
                    if(i == 0)
                    {
                        dr.Name = reader["DriverName"].ToString();
                        dr.Address = reader["Address"].ToString();
                        dr.MobileNo = reader["MobileNo"].ToString();
                        dr.Experince = int.Parse(reader["Experince"].ToString());
                        dr.ImagePath = reader["ImagePath"].ToString();
                        hist.Driver = dr;
                    }
                    i = i++;
                    rent = new Rent();
                    rent.Id = int.Parse(reader["ID"].ToString());
                    rent.PickUp = reader["PickUp"].ToString();
                    rent.DropOff = reader["DropOff"].ToString();
                    rent.PickUpDate = Convert.ToDateTime(reader["PickUpDate"].ToString());
                    rent.DropOffDate = Convert.ToDateTime(reader["DropOffDate"].ToString());
                    rent.TotalRun = int.Parse(reader["TotalRun"].ToString());
                    rent.Brand = reader["Brand"].ToString();
                    rent.Model = reader["Model"].ToString();
                    hist.Rents.Add(rent);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return hist;
        }

        [SupportedOSPlatform("windows")]
        public List<Rent> GetAllRents()
        {
            List<Rent> Rents = new List<Rent>();
            Rent rent;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = "Select * from Rents;";
                SqlDataReader reader = GetData(qry, con);
                while (reader.Read())
                {
                    rent = new Rent();
                    rent.Id = int.Parse(reader["ID"].ToString());
                    rent.PickUp = reader["PickUp"].ToString();
                    rent.DropOff = reader["DropOff"].ToString();
                    rent.PickUpDate = Convert.ToDateTime(reader["PickUpDate"].ToString());
                    rent.DropOffDate = Convert.ToDateTime(reader["DropOffDate"].ToString()) ;
                    rent.TotalRun = int.Parse(reader["TotalRun"].ToString());
                    rent.Rate = int.Parse(reader["Rate"].ToString());
                    rent.TotalAmount = int.Parse(reader["TotalAmount"].ToString());
                    rent.Brand = reader["Brand"].ToString();
                    rent.Model = reader["Model"].ToString();
                    rent.DriverId = int.Parse(reader["DriverId"].ToString());
                    rent.CustomerName = reader["CustomerName"].ToString();
                    rent.CustomerContact = reader["CustomerContactNo"].ToString();
                    Rents.Add(rent);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return Rents;
        }

        [SupportedOSPlatform("windows")]
        public List<string> GetModel(string brand)
        {
            List<string> model = new List<string>();
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = "Select distinct Model from Cars where Brand='"+ brand +"'";
                SqlDataReader reader = GetData(qry, con);
                while (reader.Read())
                {
                    model.Add(reader["Model"].ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return model;
        }


        [SupportedOSPlatform("windows")]
        public List<string> GetBrand()
        {
            List<string> brand = new List<string>();
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = "Select distinct Brand from Cars;";
                SqlDataReader reader = GetData(qry, con);
                while (reader.Read())
                {
                    brand.Add(reader["Brand"].ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return brand;
        }


        [SupportedOSPlatform("windows")]
        public bool BookingNow(Rent rent)
        {
            bool isSaved = false;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                rent.TotalAmount = rent.TotalRun * rent.Rate;
                string qry = String.Format("Insert into Rents(PickUp,DropOff,PickUpDate,DropOffDate,TotalRun,Rate,TotalAmount,Brand,Model,DriverId,CustomerName,CustomerContactNo) values(" +
                    "'{0}', '{1}', '{2}', '{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", 
                    rent.PickUp,rent.DropOff,rent.PickUpDate,rent.DropOffDate,rent.TotalRun,rent.Rate,rent.TotalAmount,rent.Brand,rent.Model,rent.DriverId,rent.CustomerName,rent.CustomerContact);
                isSaved = SaveData(qry, con);
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return isSaved;
        }


        [SupportedOSPlatform("windows")]
        public List<Driver> GetAllDrivers()
        {
            List<Driver> drivers = new List<Driver>();
            Driver dr;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = "Select * from drivers;";
                SqlDataReader reader = GetData(qry, con);
                while (reader.Read())
                {
                    dr = new Driver();
                    dr.Id = int.Parse(reader["ID"].ToString());
                    dr.Name = reader["DriverNAME"].ToString();
                    dr.Address = reader["Address"].ToString();
                    dr.MobileNo = reader["MobileNo"].ToString();
                    dr.Age = int.Parse(reader["Age"].ToString());
                    dr.Experince = int.Parse(reader["Experince"].ToString());
                    dr.ImagePath = reader["ImagePath"].ToString();
                    drivers.Add(dr);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return drivers;
        }


        [SupportedOSPlatform("windows")]
        public bool AddDriver(Driver newdriver)
        {
            bool isSaved = false;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                newdriver.ImagePath = SaveImage(newdriver.DriverImage, "drivers");
                string qry = string.Format("Insert into Drivers(DriverName,Address,MobileNo,Age,Experince,ImagePath) values("+
                    "'{0}','{1}','{2}','{3}','{4}','{5}')",newdriver.Name, newdriver.Address,newdriver.MobileNo,newdriver.Age,newdriver.Experince,newdriver.ImagePath);
                isSaved = SaveData(qry, con);
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return isSaved;
        }


        [SupportedOSPlatform("windows")]
        public List<Car> GetAllCars()
        {
            List<Car> cars = new List<Car>();
            Car car;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                string qry = "Select * from Cars";
                SqlDataReader reader = GetData(qry, con);
                while (reader.Read())
                {
                    car = new Car();
                    car.Id = int.Parse(reader["ID"].ToString());
                    car.Brand = reader["Brand"].ToString();
                    car.Model = reader["Model"].ToString();
                    car.PassingYear = int.Parse(reader["PassingYear"].ToString());
                    car.Engine = reader["Engine"].ToString();
                    car.FuelType = reader["FuelType"].ToString();
                    car.ImagePath = reader["ImagePath"].ToString();
                    car.CarNumber = reader["CarNumber"].ToString();
                    car.SeatingCapacity = int.Parse(reader["SeatingCapacity"].ToString());
                    cars.Add(car);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { con.Close(); }
            return cars;
        }


        [SupportedOSPlatform("windows")]
        private SqlDataReader GetData(string qry, SqlConnection con)
        {
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                reader = cmd.ExecuteReader();
            }
            catch (Exception)
            {

                throw;
            }
            return reader;
        }


        [SupportedOSPlatform("windows")]
        public bool AddNewCar(Car newcar)
        {
            bool isSaved = false;
            SqlConnection con = GetSqlConnection();
            try
            {
                con.Open();
                newcar.ImagePath = SaveImage(newcar.CarImage, "cars");
                string qry = string.Format("Insert into Cars(Brand,Model,PassingYear,CarNumber,Engine,FuelType,ImagePath,SeatingCapacity) values("+
                    "'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",newcar.Brand,newcar.Model,newcar.PassingYear,newcar.CarNumber,newcar.Engine,newcar.FuelType,newcar.ImagePath,newcar.SeatingCapacity);
                isSaved= SaveData(qry, con);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                con.Close();
            }
            return isSaved;
        }


        private string SaveImage(IFormFile file, string folderName)
        {
            string imagepath = "";
            try
            {
                string uploadfolder = Path.Combine(webhost.WebRootPath,"images/"+folderName);
                imagepath = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filepath = Path.Combine(uploadfolder, imagepath);
                using(var filestream = new FileStream(filepath, FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return imagepath;
        }


        [SupportedOSPlatform("windows")]// adding because sql support in windows os only
        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(dbcon);
        }


        [SupportedOSPlatform("windows")]
        private bool SaveData(string qry, SqlConnection con)
        {
            bool isSaved = false;
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.ExecuteNonQuery();
                isSaved = true;
            }
            catch (Exception)
            {

                throw;
            }
            return isSaved;
        }
    }
}
