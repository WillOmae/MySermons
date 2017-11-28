using System.Data.SQLite;
using System.IO;
using System.Text;

namespace AppEngine
{
    public static class MainThread
    {
        static public bool CheckFileExistence()
        {
            if (File.Exists(FileNames.dbLocation))
            {
                if (new Database.Theme().SelectAll() is null)
                {
                    InitializeDB();
                }
            }
            if (!File.Exists(FileNames.dbLocation))
            {
                try
                {
                    Directory.CreateDirectory(FileNames.dbFolder);
                    SQLiteConnection.CreateFile(FileNames.dbLocation);

                    InitializeDB();
                }
                catch
                {
                    return false;
                }
            }
            bool allFound = true;
            //table existence

            if (File.Exists(FileNames.tempBible))
            {
                if (File.Exists(FileNames.Bible))
                {
                    File.Delete(FileNames.Bible);
                }
                File.Move(FileNames.tempBible, FileNames.Bible);
            }
            if (!File.Exists(FileNames.Bible))
            {
                allFound = false;
            }
            //if (!Directory.Exists(FileNames.WalkthroughsDirectory))
            //{
            //    System.Windows.Forms.MessageBox.Show("Walkthroughs missing");
            //    allFound = false;
            //}
            return allFound;
        }
        static public bool InitializeDB()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(FileNames.ConnectionString))
                {
                    connection.Open();
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand("DROP TABLE IF EXISTS `VENUES`; DROP TABLE IF EXISTS `TOWNS`; DROP TABLE IF EXISTS `SPEAKERS`; DROP TABLE IF EXISTS `SERMONS`; DROP TABLE IF EXISTS `SERIESSPEAKERS`; DROP TABLE IF EXISTS `SERIES`; DROP TABLE IF EXISTS `RODs`; DROP TABLE IF EXISTS `PREFERENCES`; DROP TABLE IF EXISTS `ACTIVITIES`; DROP TABLE IF EXISTS `THEMES`;", connection)) { command.ExecuteNonQuery(); }
                        using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE `VENUES` (`Id`INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`Name` TEXT NOT NULL,`Town` INTEGER NOT NULL DEFAULT 0); CREATE TABLE `TOWNS` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`Name` TEXT NOT NULL UNIQUE); CREATE TABLE `SPEAKERS` (`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`Name` TEXT NOT NULL UNIQUE); CREATE TABLE `SERMONS` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`SERIES`	INTEGER NOT NULL DEFAULT 0,`DateCreated` TEXT NOT NULL,`Venue` INTEGER NOT NULL DEFAULT 0,`Town` INTEGER NOT NULL DEFAULT 0,`Activity`	INTEGER NOT NULL DEFAULT 0,`Speaker` INTEGER NOT NULL DEFAULT 0,`Theme` INTEGER NOT NULL DEFAULT 0,`Title`	TEXT NOT NULL,`Text` TEXT,`Hymn` TEXT,`Content` TEXT NOT NULL); CREATE TABLE `SERIESSPEAKERS` (`SeriesId` INTEGER NOT NULL,`SpeakerId` INTEGER NOT NULL, PRIMARY KEY(`SeriesId`,`SpeakerId`)); CREATE TABLE `SERIES` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`Theme` TEXT NOT NULL,`Speaker` INTEGER NOT NULL DEFAULT 0,`Venue`	INTEGER NOT NULL DEFAULT 0,`Town` INTEGER NOT NULL DEFAULT 0,`Activity` INTEGER NOT NULL DEFAULT 0,`StartDate`	TEXT NOT NULL,`EndDate`	TEXT NOT NULL); CREATE TABLE `RODs` (`DocId` INTEGER NOT NULL,`DocTitle`	TEXT,PRIMARY KEY(`DocId`)); CREATE TABLE `PREFERENCES` (`PrinterName`	TEXT,`PrinterScheme` TEXT DEFAULT 'White/Black',`ColourFont` TEXT DEFAULT 'AACCFF',`ColourControls` TEXT DEFAULT 222233,`FontSystem` TEXT DEFAULT 'Times New Roman',`FontReader` TEXT DEFAULT 'Times New Roman',`FontWriter` TEXT DEFAULT 'Times New Roman',`RODMaxNumber` INTEGER DEFAULT 10,`SortingFilter` TEXT DEFAULT 'SPEAKER',`ShowWelcomeScreen` TEXT DEFAULT 'True'); CREATE TABLE `ACTIVITIES` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`Name` TEXT NOT NULL UNIQUE); CREATE TABLE `THEMES` (`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`Name` TEXT NOT NULL UNIQUE); ", connection)) { command.ExecuteNonQuery(); }
                        using (SQLiteCommand command = new SQLiteCommand("INSERT INTO `PREFERENCES` (PrinterName,PrinterScheme,ColourFont,ColourControls,FontSystem,FontReader,FontWriter,RODMaxNumber,SortingFilter,ShowWelcomeScreen) VALUES ('Microsoft XPS Document Writer','','170204255','034034051','Times New Roman','Times New Roman','Times New Roman',10,'Venue','True');", connection)) { command.ExecuteNonQuery(); }
                        using (SQLiteCommand command = new SQLiteCommand("INSERT INTO `TOWNS` (Id,Name) VALUES (0,'none'); INSERT INTO `TOWNS` (Id, Name) VALUES(1, 'Nairobi');INSERT INTO `TOWNS` (Id, Name) VALUES(2,'Mombasa');INSERT INTO `TOWNS` (Id, Name) VALUES(3,'Kisumu');INSERT INTO `TOWNS` (Id, Name) VALUES(4,'Nakuru');INSERT INTO `TOWNS` (Id, Name) VALUES(5,'Eldoret');INSERT INTO `TOWNS` (Id, Name) VALUES(6,'Kehancha');INSERT INTO `TOWNS` (Id, Name) VALUES(7,'Ruiru');INSERT INTO `TOWNS` (Id, Name) VALUES(8,'Kikuyu');INSERT INTO `TOWNS` (Id, Name) VALUES(9,'Kangundo-Tala');INSERT INTO `TOWNS` (Id, Name) VALUES(10,'Malindi');INSERT INTO `TOWNS` (Id, Name) VALUES(11,'Naivasha');INSERT INTO `TOWNS` (Id, Name) VALUES(12,'Kitui');INSERT INTO `TOWNS` (Id, Name) VALUES(13,'Machakos');INSERT INTO `TOWNS` (Id, Name) VALUES(14,'Thika');INSERT INTO `TOWNS` (Id, Name) VALUES(15,'Athi River (Mavoko)');INSERT INTO `TOWNS` (Id, Name) VALUES(16,'Karuri');INSERT INTO `TOWNS` (Id, Name) VALUES(17,'Nyeri');INSERT INTO `TOWNS` (Id, Name) VALUES(18,'Kilifi');INSERT INTO `TOWNS` (Id, Name) VALUES(19,'Garissa');INSERT INTO `TOWNS` (Id, Name) VALUES(20,'Vihiga');INSERT INTO `TOWNS` (Id, Name) VALUES(21,'Mumias');INSERT INTO `TOWNS` (Id, Name) VALUES(22,'Bomet');INSERT INTO `TOWNS` (Id, Name) VALUES(23,'Molo');INSERT INTO `TOWNS` (Id, Name) VALUES(24,'Ngong');INSERT INTO `TOWNS` (Id, Name) VALUES(25,'Kitale');INSERT INTO `TOWNS` (Id, Name) VALUES(26,'Litein');INSERT INTO `TOWNS` (Id, Name) VALUES(27,'Limuru');INSERT INTO `TOWNS` (Id, Name) VALUES(28,'Kericho');INSERT INTO `TOWNS` (Id, Name) VALUES(29,'Kimilili');INSERT INTO `TOWNS` (Id, Name) VALUES(30,'Awasi');INSERT INTO `TOWNS` (Id, Name) VALUES(31,'Kakamega');INSERT INTO `TOWNS` (Id, Name) VALUES(32,'Kapsabet');INSERT INTO `TOWNS` (Id, Name) VALUES(33,'Mariakani');INSERT INTO `TOWNS` (Id, Name) VALUES(34,'Kiambu');INSERT INTO `TOWNS` (Id, Name) VALUES(35,'Mandera');INSERT INTO `TOWNS` (Id, Name) VALUES(36,'Nyamira');INSERT INTO `TOWNS` (Id, Name) VALUES(37,'Mwingi');INSERT INTO `TOWNS` (Id, Name) VALUES(38,'Kisii');INSERT INTO `TOWNS` (Id, Name) VALUES(39,'Wajir');INSERT INTO `TOWNS` (Id, Name) VALUES(40,'Rongo');INSERT INTO `TOWNS` (Id, Name) VALUES(41,'Bungoma');INSERT INTO `TOWNS` (Id, Name) VALUES(42,'Ahero');INSERT INTO `TOWNS` (Id, Name) VALUES(43,'Nandi Hills');INSERT INTO `TOWNS` (Id, Name) VALUES(44,'Makuyu');INSERT INTO `TOWNS` (Id, Name) VALUES(45,'Kapenguria');INSERT INTO `TOWNS` (Id, Name) VALUES(46,'Taveta');INSERT INTO `TOWNS` (Id, Name) VALUES(47,'Narok');INSERT INTO `TOWNS` (Id, Name) VALUES(48,'Ol Kalou');INSERT INTO `TOWNS` (Id, Name) VALUES(49,'Kakuma');INSERT INTO `TOWNS` (Id, Name) VALUES(50,'Webuye');INSERT INTO `TOWNS` (Id, Name) VALUES(51,'Malaba');INSERT INTO `TOWNS` (Id, Name) VALUES(52,'Mbita Point');INSERT INTO `TOWNS` (Id, Name) VALUES(53,'Ukunda');INSERT INTO `TOWNS` (Id, Name) VALUES(54,'Wundanyi');INSERT INTO `TOWNS` (Id, Name) VALUES(55,'Busia');INSERT INTO `TOWNS` (Id, Name) VALUES(56,'Runyenjes');INSERT INTO `TOWNS` (Id, Name) VALUES(57,'Migori');INSERT INTO `TOWNS` (Id, Name) VALUES(58,'Malava');INSERT INTO `TOWNS` (Id, Name) VALUES(59,'Suneka');INSERT INTO `TOWNS` (Id, Name) VALUES(60,'Embu');INSERT INTO `TOWNS` (Id, Name) VALUES(61,'Ogembo');INSERT INTO `TOWNS` (Id, Name) VALUES(62,'Homa Bay');INSERT INTO `TOWNS` (Id, Name) VALUES(63,'Lodwar');INSERT INTO `TOWNS` (Id, Name) VALUES(64,'Kitengela');INSERT INTO `TOWNS` (Id, Name) VALUES(65,'Ukwala');INSERT INTO `TOWNS` (Id, Name) VALUES(66,'Keroka');INSERT INTO `TOWNS` (Id, Name) VALUES(67,'Meru');INSERT INTO `TOWNS` (Id, Name) VALUES(68,'Matuu');INSERT INTO `TOWNS` (Id, Name) VALUES(69,'Oyugis');INSERT INTO `TOWNS` (Id, Name) VALUES(70,'Nyahururu');INSERT INTO `TOWNS` (Id, Name) VALUES(71,'Kipkelion');INSERT INTO `TOWNS` (Id, Name) VALUES(72,'Luanda');INSERT INTO `TOWNS` (Id, Name) VALUES(73,'Nanyuki');INSERT INTO `TOWNS` (Id, Name) VALUES(74,'Maua');INSERT INTO `TOWNS` (Id, Name) VALUES(75,'Mtwapa');INSERT INTO `TOWNS` (Id, Name) VALUES(76,'Isiolo');INSERT INTO `TOWNS` (Id, Name) VALUES(77,'Eldama Ravine');INSERT INTO `TOWNS` (Id, Name) VALUES(78,'Voi');INSERT INTO `TOWNS` (Id, Name) VALUES(79,'Siaya');INSERT INTO `TOWNS` (Id, Name) VALUES(80,'Nyansiongo');INSERT INTO `TOWNS` (Id, Name) VALUES(81,'Londiani');INSERT INTO `TOWNS` (Id, Name) VALUES(82,'Iten/Tambach');INSERT INTO `TOWNS` (Id, Name) VALUES(83,'Chuka');INSERT INTO `TOWNS` (Id, Name) VALUES(84,'Malakisi');INSERT INTO `TOWNS` (Id, Name) VALUES(85,'Juja');INSERT INTO `TOWNS` (Id, Name) VALUES(86,'Ongata Rongai');INSERT INTO `TOWNS` (Id, Name) VALUES(87,'Bondo');INSERT INTO `TOWNS` (Id, Name) VALUES(88,'Moyale');INSERT INTO `TOWNS` (Id, Name) VALUES(89,'Maralal');INSERT INTO `TOWNS` (Id, Name) VALUES(90,'Gilgil');INSERT INTO `TOWNS` (Id, Name) VALUES(91,'Nambale');INSERT INTO `TOWNS` (Id, Name) VALUES(92,'Tabaka');INSERT INTO `TOWNS` (Id, Name) VALUES(93,'Muhoroni');INSERT INTO `TOWNS` (Id, Name) VALUES(94,'Kerugoya/Kutus');INSERT INTO `TOWNS` (Id, Name) VALUES(95,'Ugunja');INSERT INTO `TOWNS` (Id, Name) VALUES(96,'Yala');INSERT INTO `TOWNS` (Id, Name) VALUES(97,'Rumuruti');INSERT INTO `TOWNS` (Id, Name) VALUES(98,'Burnt Forest');INSERT INTO `TOWNS` (Id, Name) VALUES(99,'Maragua');INSERT INTO `TOWNS` (Id, Name) VALUES(100,'Kendu Bay');INSERT INTO `TOWNS` (Id,Name) VALUES (101,'Archerspost');INSERT INTO `TOWNS` (Id,Name) VALUES (102,'Bahati');INSERT INTO `TOWNS` (Id,Name) VALUES (103,'Baragoi');INSERT INTO `TOWNS` (Id,Name) VALUES (104,'Baringo');INSERT INTO `TOWNS` (Id,Name) VALUES (105,'Bissil');INSERT INTO `TOWNS` (Id,Name) VALUES (106,'Bumala');INSERT INTO `TOWNS` (Id,Name) VALUES (107,'Butere');INSERT INTO `TOWNS` (Id,Name) VALUES (108,'Cheptais');INSERT INTO `TOWNS` (Id,Name) VALUES (109,'Chogoria');INSERT INTO `TOWNS` (Id,Name) VALUES (110,'Daadab');INSERT INTO `TOWNS` (Id,Name) VALUES (111,'Dundori');INSERT INTO `TOWNS` (Id,Name) VALUES (112,'Endarasha');INSERT INTO `TOWNS` (Id,Name) VALUES (113,'Funyula');INSERT INTO `TOWNS` (Id,Name) VALUES (114,'Garbatula');INSERT INTO `TOWNS` (Id,Name) VALUES (115,'Gatundu');INSERT INTO `TOWNS` (Id,Name) VALUES (116,'Hola');INSERT INTO `TOWNS` (Id,Name) VALUES (117,'Iten');INSERT INTO `TOWNS` (Id,Name) VALUES (118,'Juakali');INSERT INTO `TOWNS` (Id,Name) VALUES (119,'Kabarnet');INSERT INTO `TOWNS` (Id,Name) VALUES (120,'Kabati');INSERT INTO `TOWNS` (Id,Name) VALUES (121,'Kabuti');INSERT INTO `TOWNS` (Id,Name) VALUES (122,'Kagio');INSERT INTO `TOWNS` (Id,Name) VALUES (123,'Kagumo');INSERT INTO `TOWNS` (Id,Name) VALUES (124,'Kajiado');INSERT INTO `TOWNS` (Id,Name) VALUES (125,'Kaloleni');INSERT INTO `TOWNS` (Id,Name) VALUES (126,'Kandara');INSERT INTO `TOWNS` (Id,Name) VALUES (127,'Kangari');INSERT INTO `TOWNS` (Id,Name) VALUES (128,'Kangema');INSERT INTO `TOWNS` (Id,Name) VALUES (129,'Kangundo');INSERT INTO `TOWNS` (Id,Name) VALUES (130,'Kapcherop');INSERT INTO `TOWNS` (Id,Name) VALUES (131,'Kapsokwony');INSERT INTO `TOWNS` (Id,Name) VALUES (132,'Kapsowar');INSERT INTO `TOWNS` (Id,Name) VALUES (133,'Karatina');INSERT INTO `TOWNS` (Id,Name) VALUES (134,'Kathiani');INSERT INTO `TOWNS` (Id,Name) VALUES (135,'Kibwezi');INSERT INTO `TOWNS` (Id,Name) VALUES (136,'Kinamba');INSERT INTO `TOWNS` (Id,Name) VALUES (137,'Kinna');INSERT INTO `TOWNS` (Id,Name) VALUES (138,'Kiria-Ini');INSERT INTO `TOWNS` (Id,Name) VALUES (139,'Laisamis');INSERT INTO `TOWNS` (Id,Name) VALUES (140,'Lamu');INSERT INTO `TOWNS` (Id,Name) VALUES (141,'Lare');INSERT INTO `TOWNS` (Id,Name) VALUES (142,'Loiyangalani');INSERT INTO `TOWNS` (Id,Name) VALUES (143,'Lokichogio');INSERT INTO `TOWNS` (Id,Name) VALUES (144,'Lolgorian');INSERT INTO `TOWNS` (Id,Name) VALUES (145,'Lungalunga');INSERT INTO `TOWNS` (Id,Name) VALUES (146,'Machinery');INSERT INTO `TOWNS` (Id,Name) VALUES (147,'Magarini');INSERT INTO `TOWNS` (Id,Name) VALUES (148,'Majimazuri');INSERT INTO `TOWNS` (Id,Name) VALUES (149,'Marereni');INSERT INTO `TOWNS` (Id,Name) VALUES (150,'Maseno');INSERT INTO `TOWNS` (Id,Name) VALUES (151,'Masii');INSERT INTO `TOWNS` (Id,Name) VALUES (152,'Maunarok');INSERT INTO `TOWNS` (Id,Name) VALUES (153,'Mazeras');INSERT INTO `TOWNS` (Id,Name) VALUES (154,'Merti');INSERT INTO `TOWNS` (Id,Name) VALUES (155,'Mitunguu');INSERT INTO `TOWNS` (Id,Name) VALUES (156,'Mogonga');INSERT INTO `TOWNS` (Id,Name) VALUES (157,'Mogotio');INSERT INTO `TOWNS` (Id,Name) VALUES (158,'Mtito Andei');INSERT INTO `TOWNS` (Id,Name) VALUES (159,'Muhuru Bay');INSERT INTO `TOWNS` (Id,Name) VALUES (160,'Muranga');INSERT INTO `TOWNS` (Id,Name) VALUES (161,'Mwatate');INSERT INTO `TOWNS` (Id,Name) VALUES (162,'Mweiga');INSERT INTO `TOWNS` (Id,Name) VALUES (163,'Nairagieenkare');INSERT INTO `TOWNS` (Id,Name) VALUES (164,'Naromoru');INSERT INTO `TOWNS` (Id,Name) VALUES (165,'Ndori');INSERT INTO `TOWNS` (Id,Name) VALUES (166,'Ngong’');INSERT INTO `TOWNS` (Id,Name) VALUES (167,'Njabini');INSERT INTO `TOWNS` (Id,Name) VALUES (168,'Nyakach');INSERT INTO `TOWNS` (Id,Name) VALUES (169,'Nyamache');INSERT INTO `TOWNS` (Id,Name) VALUES (170,'Nyamarambe');INSERT INTO `TOWNS` (Id,Name) VALUES (171,'Olenguruone');INSERT INTO `TOWNS` (Id,Name) VALUES (172,'Othaya');INSERT INTO `TOWNS` (Id,Name) VALUES (173,'Rongai');INSERT INTO `TOWNS` (Id,Name) VALUES (174,'Salgaa');INSERT INTO `TOWNS` (Id,Name) VALUES (175,'Siakago');INSERT INTO `TOWNS` (Id,Name) VALUES (176,'Sindo');INSERT INTO `TOWNS` (Id,Name) VALUES (177,'Sololo');INSERT INTO `TOWNS` (Id,Name) VALUES (178,'Sultan Hamud');INSERT INTO `TOWNS` (Id,Name) VALUES (179,'Timboroa');INSERT INTO `TOWNS` (Id,Name) VALUES (180,'Tongaren');INSERT INTO `TOWNS` (Id,Name) VALUES (181,'Watamu');INSERT INTO `TOWNS` (Id,Name) VALUES (182,'Wote');INSERT INTO `TOWNS` (Id,Name) VALUES (183,'Kahawa');", connection)) { command.ExecuteNonQuery(); }
                        using (SQLiteCommand command = new SQLiteCommand("INSERT INTO `VENUES` (Id,Name,Town) VALUES (0,'none',0);", connection)) { command.ExecuteNonQuery(); }
                        using (SQLiteCommand command = new SQLiteCommand("INSERT INTO `SPEAKERS` (Id,Name) VALUES (0,'none');", connection)) { command.ExecuteNonQuery(); }
                        using (SQLiteCommand command = new SQLiteCommand("INSERT INTO `SERIES` (Id,Theme,Speaker,Venue,Town,Activity,StartDate,EndDate) VALUES (0,'none',0,0,0,0,'2017/07/09 00:00:00','2017/07/09 00:00:00');", connection)) { command.ExecuteNonQuery(); }
                        using (SQLiteCommand command = new SQLiteCommand("INSERT INTO `ACTIVITIES` (Id,Name) VALUES (0,'none');", connection)) { command.ExecuteNonQuery(); }
                        transaction.Commit();
                    }
                }
                return true;
            }
            catch (System.Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                return false;
            }
        }
        static public string CreateUniqueNameFromArray(string[] arrayComponents)
        {
            StringBuilder szUniqueString1 = new StringBuilder();
            arrayComponents.SetValue("00", 0);//Set the value of the first string to "00" to fix a bug in which different the same doc would be opened twice due to different indices in the listview

            int count = 1;
            foreach (string component in arrayComponents)
            {
                if (count >= 1)
                    szUniqueString1.Append(component);
                count++;
            }

            return RemoveAllSpacesFromString(szUniqueString1.ToString());
        }
        static public string RemoveAllSpacesFromString(string szInitialString)
        {
            return szInitialString.ToString().Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }
    }
}