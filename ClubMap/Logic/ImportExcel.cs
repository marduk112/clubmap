using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClubMap.Common;
using ClubMap.DbModels.Sqlite;
using ClubMap.Models;
using Excel;

namespace ClubMap.Logic
{
    public class ImportExcel
    {
        public static void ImportFixedData(string path)
        {
            //var excelFile = new ExcelQueryFactory(path) { TrimSpaces = TrimSpacesType.Both, ReadOnly = true };
            //foreach (var row in excelFile.GetWorksheetNames()
            //    .Select(worksheet => (from row in excelFile.Worksheet(worksheet) select row)).SelectMany(rows => rows))
            {
                using (var ds = FillDataSet(path))
                {
                    for (var i = 1; i < ds.Tables[0].Rows.Count; i++)
                    {
                        using (var db = new SqLiteDbContext())
                        {
                            var local = ds.Tables[0].Rows[i][1].ToString();
                            switch (local)
                            {
                                case "Klub":
                                    var club = new Club();
                                    SetCommonData(club, ds.Tables[0].Rows[i], db);
                                    SetClubData(club, ds.Tables[0].Rows[i], db);
                                    db.Clubs.AddOrUpdate(x => x.Code, club);
                                    break;
                                case "Pub":
                                    var pub = new Pub();
                                    SetCommonData(pub, ds.Tables[0].Rows[i], db);
                                    SetPubData(pub, ds.Tables[0].Rows[i], db);
                                    db.Pubs.AddOrUpdate(x => x.Code, pub);
                                    break;
                                case "Restaurant":
                                    var restaurant = new Restaurant();
                                    SetCommonData(restaurant, ds.Tables[0].Rows[i], db);
                                    SetRestaurantData(restaurant, ds.Tables[0].Rows[i], db);
                                    db.Restaurants.AddOrUpdate(x => x.Code, restaurant);
                                    break;
                                case "Kawiarnie":
                                    var café = new Cafe();
                                    SetCommonData(café, ds.Tables[0].Rows[i], db);
                                    SetCaféData(café, ds.Tables[0].Rows[i], db);
                                    db.Cafes.AddOrUpdate(x => x.Code, café);
                                    break;
                                case "Hotel":
                                    var hotel = new Hotel();
                                    SetCommonData(hotel, ds.Tables[0].Rows[i], db);
                                    SetHotelData(hotel, ds.Tables[0].Rows[i], db);
                                    db.Hotels.AddOrUpdate(x => x.Code, hotel);
                                    break;
                            }
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        public static void ImportVariableData(string path)
        {
            var id = 1;
            using (var ds = FillDataSet(path))
            {
                for (var i = 1; i < ds.Tables[0].Rows.Count; i++)
                {
                    using (var db = new SqLiteDbContext())
                    {
                        var row = ds.Tables[0].Rows[i];
                        var data = new VariableQueueData
                        {
                            Id = id++,
                            Hour = row[2].ToString(),
                            People = IntValue(row[5].ToString()),
                            WomanPercent = row[7].ToString(),
                            ManPercent = row[8].ToString(),
                            QueueTime = IntValue(row[9].ToString()),
                            BarTime = IntValue(row[11].ToString()),
                            PeopleArrow = row[6].ToString(),
                            QueueArrow = row[10].ToString(),
                            DayId = Constants.Days[row[1].ToString()]
                        };
                        //Search for code of cafe, restaurant pub or ...
                        if (!string.IsNullOrEmpty(row[0].ToString()))
                        {
                            string code = row[0].ToString();
                            //first letter of code is a local type
                            switch (code[0])
                            {
                                case 'K':
                                    data.ClubCode = code;
                                    break;
                                case 'P':
                                    data.PubCode = code;
                                    break;
                                case 'R':
                                    data.RestaurantCode = code;
                                    break;
                                case 'A':
                                    data.CafeCode = code;
                                    break;
                                case 'H':
                                    data.HotelCode = code;
                                    break;
                            }
                        }
                        db.VariableQueueDatas.AddOrUpdate(x => x.Id, data);
                        db.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Fill the data set.
        /// </summary>
        /// <param name="pathToExcelFile">The pathToExcelFile.</param>
        /// <returns></returns>
        private static DataSet FillDataSet(string pathToExcelFile)
        {
            DataSet ds = null;
            IExcelDataReader excelReader = null;
            //http://www.aspsnippets.com/Articles/Read-and-Import-Excel-File-into-DataSet-or-DataTable-using-C-and-VBNet-in-ASPNet.aspx
            using (var stream = new FileStream(pathToExcelFile, FileMode.Open))
            {
                if (Path.GetExtension(pathToExcelFile).Equals(".xlsx"))
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                else if (Path.GetExtension(pathToExcelFile).Equals(".xls"))
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream, ReadOption.Loose);
                if (excelReader != null) ds = excelReader.AsDataSet();
            }
            return ds;
        }
        private static void SetHotelData(Hotel hotel, DataRow row, SqLiteDbContext db)
        {
            hotel.AvailableCoupons = row[33].ToString().ToLower().Equals("yes");
            string price = row[59].ToString();
            var hotelPrice = db.Prices.FirstOrDefault(x => x.Name.Equals(price));
            if (hotelPrice != null)
            {
                var hotelPriceId = hotelPrice.Id;
                hotel.HotelPriceId = hotelPriceId;
            }
            else
                hotel.HotelPrice = new Price {Name = price};
            
            hotel.Checkin = row[60].ToString();
            hotel.Checkout = row[61].ToString();
            hotel.Opinion = double.Parse(row[62].ToString());
            hotel.RoomPrice = uint.Parse(row[63].ToString());
            hotel.Stars = uint.Parse(row[64].ToString());
            hotel.Parking = row[65].ToString().ToLower().Equals("for free") ? 0 : DoubleValue(row[65].ToString());
            hotel.Internet = row[66].ToString().ToLower().Equals("for free") ? 0 : DoubleValue(row[66].ToString());
            hotel.Kid = row[67].ToString();
            hotel.Animals = row[68].ToString().ToLower().Equals("accepted");
            hotel.PicturesLink = row[69].ToString();
        }

        private static void SetCaféData(Cafe cafe, DataRow row, SqLiteDbContext db)
        {
            cafe.MondayOpening = row[8].ToString();
            cafe.MondayClosing = row[9].ToString();
            cafe.ThusdayClosing = row[10].ToString();
            cafe.ThusdayOpening = row[11].ToString();
            cafe.WednesdayOpening = row[12].ToString();
            cafe.WednesdayClosing = row[13].ToString();
            cafe.ThursdayOpening = row[14].ToString();
            cafe.ThursdayClosing = row[15].ToString();
            cafe.FridayOpening = row[16].ToString();
            cafe.FridayClosing = row[17].ToString();
            cafe.SaturdayOpening = row[18].ToString();
            cafe.SaturdayClosing = row[19].ToString();
            cafe.SundayOpening = row[20].ToString();
            cafe.SundayClosing = row[21].ToString();
            cafe.AvailableCoupons = row[33].ToString().ToLower().Equals("yes");
            string c = row[49].ToString();
            var cafeType = db.CafeTypes.FirstOrDefault(x => x.Name.Equals(c));
            if (cafeType != null)
            {
                var cafeTypeId = cafeType.Id;
                cafe.CafeTypeId = cafeTypeId;
            }
            else
                cafe.CafeType = new CafeType {Name = c};
            cafe.Tea = DoubleValue(row[50].ToString());
            cafe.BlackCoffee = row[51].ToString().Equals("none") ? null : DoubleValue(row[51].ToString());
            cafe.WhiteCoffee = row[52].ToString().Equals("none") ? null : DoubleValue(row[52].ToString());
            cafe.Espresso = row[53].ToString().Equals("none") ? null : DoubleValue(row[53].ToString());
            cafe.Capuccino = row[54].ToString().Equals("none") ? null : DoubleValue(row[54].ToString());
            cafe.Frappucino = row[55].ToString().Equals("none") ? null : DoubleValue(row[55].ToString());
            cafe.DessertsLow = row[56].ToString().Equals("none") ? null : DoubleValue(row[56].ToString());
            cafe.DessertsHigh = row[57].ToString().Equals("none") ? null : DoubleValue(row[57].ToString());
            cafe.HotDishes = row[58].ToString().ToLower().Equals("yes");
        }

        private static void SetRestaurantData(Restaurant restaurant, DataRow row, SqLiteDbContext db)
        {
            restaurant.MondayOpening = row[8].ToString();
            restaurant.MondayClosing = row[9].ToString();
            restaurant.ThusdayClosing = row[10].ToString();
            restaurant.ThusdayOpening = row[11].ToString();
            restaurant.WednesdayOpening = row[12].ToString();
            restaurant.WednesdayClosing = row[13].ToString();
            restaurant.ThursdayOpening = row[14].ToString();
            restaurant.ThursdayClosing = row[15].ToString();
            restaurant.FridayOpening = row[16].ToString();
            restaurant.FridayClosing = row[17].ToString();
            restaurant.SaturdayOpening = row[18].ToString();
            restaurant.SaturdayClosing = row[19].ToString();
            restaurant.SundayOpening = row[20].ToString();
            restaurant.SundayClosing = row[21].ToString();
            restaurant.AvailableCoupons = row[33].ToString().ToLower().Equals("yes");
            string food = row[45].ToString();
            var foodType = db.FoodTypes.FirstOrDefault(x => x.TypeOfFood.Equals(food));
            if (foodType != null)
            {
                var foodTypeId = foodType.Id;
                restaurant.FoodTypeId = foodTypeId;
            }
            else
                restaurant.FoodType = new FoodType {TypeOfFood = food};
            restaurant.PricesRange1 = row[46].ToString();
            restaurant.PricesRange2 = row[47].ToString();
            restaurant.CheckMenu = row[48].ToString();
        }

        private static void SetPubData(Pub pub, DataRow row, SqLiteDbContext db)
        {
            pub.MondayOpening = row[8].ToString();
            pub.MondayClosing = row[9].ToString();
            pub.ThusdayClosing = row[10].ToString();
            pub.ThusdayOpening = row[11].ToString();
            pub.WednesdayOpening = row[12].ToString();
            pub.WednesdayClosing = row[13].ToString();
            pub.ThursdayOpening = row[14].ToString();
            pub.ThursdayClosing = row[15].ToString();
            pub.FridayOpening = row[16].ToString();
            pub.FridayClosing = row[17].ToString();
            pub.SaturdayOpening = row[18].ToString();
            pub.SaturdayClosing = row[19].ToString();
            pub.SundayOpening = row[20].ToString();
            pub.SundayClosing = row[21].ToString();
            pub.AvailableCoupons = row[33].ToString().ToLower().Equals("yes");
            pub.AverageAge = DoubleValue(row[32].ToString());
            pub.Juice = DoubleValue(row[34].ToString());
            pub.BeerG = DoubleValue(row[35].ToString());
            pub.BeerBootle = DoubleValue(row[36].ToString());
            pub.VS = DoubleValue(row[37].ToString());
            pub.VB = DoubleValue(row[38].ToString());
            pub.VBJ = DoubleValue(row[39].ToString());
            pub.Drink1 = DoubleValue(row[40].ToString());
            pub.Drink2 = DoubleValue(row[41].ToString());
            pub.Drink3 = DoubleValue(row[42].ToString());
            string price = row[43].ToString();
            var barPrice = db.Prices.FirstOrDefault(x => x.Name.Equals(price));
            if (barPrice != null)
            {
                var barPriceId = barPrice.Id;
                pub.BarPriceId = barPriceId;
            }
            else
                pub.Price = new Price { Name = price};

            price = row[44].ToString();
            barPrice = db.Prices.FirstOrDefault(x => x.Name.Equals(price));
            if (barPrice != null)
            {
                var barMenuPriceId = barPrice.Id;
                pub.BarMenuPriceId = barMenuPriceId;
            }
            else
                pub.MenuPrice = new Price { Name = price };
        }

        private static void SetClubData(Club club, DataRow row, SqLiteDbContext db)
        {
            club.MondayOpening = row[8].ToString();
            club.MondayClosing = row[9].ToString();
            club.ThusdayClosing = row[10].ToString();
            club.ThusdayOpening = row[11].ToString();
            club.WednesdayOpening = row[12].ToString();
            club.WednesdayClosing = row[13].ToString();
            club.ThursdayOpening = row[14].ToString();
            club.ThursdayClosing = row[15].ToString();
            club.FridayOpening = row[16].ToString();
            club.FridayClosing = row[17].ToString();
            club.SaturdayOpening = row[18].ToString();
            club.SaturdayClosing = row[19].ToString();
            club.SundayOpening = row[20].ToString();
            club.SundayClosing = row[21].ToString();
            string music = row[22].ToString();
            var musicKind = db.MusicKinds.FirstOrDefault(x => x.KindOfMusic.Equals(music));
            if (musicKind != null)
            {
                var musicKindId = musicKind.Id;
                club.MusicKindId = musicKindId;
            }
            else
                club.MusicKind = new MusicKind {KindOfMusic = music};

            string age = row[23].ToString();
            var ageKind = db.AgeKinds.FirstOrDefault(x => x.KindOfAge.Equals(age));
            if (ageKind != null)
            {
                var kindId = ageKind.Id;
                club.AgeKindId = kindId;
            }
            else
                club.AgeKind = new AgeKind {KindOfAge = age};

            string price = row[24].ToString();
            var enterPrice = db.EnterPriceDays.FirstOrDefault(x => x.Price.Equals(price));
            if (enterPrice != null)
            {
                var enterPriceId = enterPrice.Id;
                club.EnterPriceMondayId = enterPriceId;
            }
            else
                club.EnterPriceMonday = new EnterPriceDay {Price = price};

            price = row[25].ToString();
            enterPrice = db.EnterPriceDays.FirstOrDefault(x => x.Price.Equals(price));
            if (enterPrice != null)
            {
                var enterPriceId = enterPrice.Id;
                club.EnterPriceThusdayId = enterPriceId;
            }
            else
                club.EnterPriceThusday = new EnterPriceDay { Price = price };

            price = row[26].ToString();
            enterPrice = db.EnterPriceDays.FirstOrDefault(x => x.Price.Equals(price));
            if (enterPrice != null)
            {
                var enterPriceId = enterPrice.Id;
                club.EnterPriceWednesdayId = enterPriceId;
            }
            else
                club.EnterPriceWednesday = new EnterPriceDay { Price = price };

            price = row[27].ToString();
            enterPrice = db.EnterPriceDays.FirstOrDefault(x => x.Price.Equals(price));
            if (enterPrice != null)
            {
                var enterPriceId = enterPrice.Id;
                club.EnterPriceThursdayId = enterPriceId;
            }
            else
                club.EnterPriceThursday = new EnterPriceDay { Price = price }; ;

            price = row[28].ToString();
            enterPrice = db.EnterPriceDays.FirstOrDefault(x => x.Price.Equals(price));
            if (enterPrice != null)
            {
                var enterPriceId = enterPrice.Id;
                club.EnterPriceFridayId = enterPriceId;
            }
            else
                club.EnterPriceFriday = new EnterPriceDay { Price = price };

            price = row[29].ToString();
            enterPrice = db.EnterPriceDays.FirstOrDefault(x => x.Price.Equals(price));
            if (enterPrice != null)
            {
                var enterPriceId = enterPrice.Id;
                club.EnterPriceSaturdayId = enterPriceId;
            }
            else
                club.EnterPriceSaturday = new EnterPriceDay { Price = price };

            price = row[30].ToString();
            enterPrice = db.EnterPriceDays.FirstOrDefault(x => x.Price.Equals(price));
            if (enterPrice != null)
            {
                var enterPriceId = enterPrice.Id;
                club.EnterPriceSundayId = enterPriceId;
            }
            else
                club.EnterPriceSunday = new EnterPriceDay { Price = price };
            club.CoatroomPrice = IntValue(row[31].ToString());
            club.AverageAge = DoubleValue(row[32].ToString());
            club.AvailableCoupons = row[33].ToString().ToLower().Equals("yes");
            club.Juice = DoubleValue(row[34].ToString());
            club.BeerG = DoubleValue(row[35].ToString());
            club.BeerBootle = DoubleValue(row[36].ToString());
            club.VS = DoubleValue(row[37].ToString());
            club.VB = DoubleValue(row[38].ToString());
            club.VBJ = DoubleValue(row[39].ToString());
            club.Drink1 = DoubleValue(row[40].ToString());
            club.Drink2 = DoubleValue(row[41].ToString());
            club.Drink3 = DoubleValue(row[42].ToString());
        }

        private static void SetCommonData(ModelBase local, DataRow row, SqLiteDbContext db)
        {
            local.Code = row[0].ToString();
            string town = row[2].ToString();
            var firstOrDefault = db.Towns.FirstOrDefault(x => x.Name.Equals(town));
            if (firstOrDefault != null)
            {
                var townId = firstOrDefault.Id;
                local.TownId = townId;
            }
            else
                local.Town = new Town {Name = town};
            local.Name = row[3].ToString();
            local.Address = row[4].ToString();
            string coord = row[5].ToString();
            var coordinates = coord.Split(',');
            local.Latitude = double.Parse(coordinates[0].Replace(" ", ""));
            local.Longitude = double.Parse(coordinates[1].Replace(" ", ""));
            local.Phone = row[6].ToString();
            local.Rank = int.Parse(row[7].ToString());
        }

        private static double? DoubleValue(string value)
        {
            if (value == null || value.Equals("")) return null;
            var result = value.Replace(",", ".").Replace("%", "");
            return double.Parse(result);
        }

        private static int? IntValue(string value)
        {
            if (value == null || value.Equals("")) return null;
            var result = value.Replace(",", ".").Replace("%", "");
            return int.Parse(result);
        }
    }
}