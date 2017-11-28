BEGIN TRANSACTION;
CREATE TABLE "Venues" (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Name`	TEXT NOT NULL,
	`Town`	INTEGER NOT NULL DEFAULT 0,
	FOREIGN KEY(`Town`) REFERENCES `Towns`(`Id`)
);
INSERT INTO `Venues` (Id,Name,Town) VALUES (0,'_default_',0);
INSERT INTO `Venues` (Id,Name,Town) VALUES (1,'Home',87);
INSERT INTO `Venues` (Id,Name,Town) VALUES (2,'Mangu High School',185);
CREATE TABLE `Towns` (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Name`	TEXT NOT NULL UNIQUE
);
INSERT INTO `Towns` (Id,Name) VALUES (0,'_default_');
INSERT INTO `Towns` (Id,Name) VALUES (3,'Nairobi');
INSERT INTO `Towns` (Id,Name) VALUES (4,'Mombasa');
INSERT INTO `Towns` (Id,Name) VALUES (5,'Kisumu');
INSERT INTO `Towns` (Id,Name) VALUES (6,'Nakuru');
INSERT INTO `Towns` (Id,Name) VALUES (7,'Eldoret');
INSERT INTO `Towns` (Id,Name) VALUES (8,'Kehancha');
INSERT INTO `Towns` (Id,Name) VALUES (9,'Ruiru');
INSERT INTO `Towns` (Id,Name) VALUES (10,'Kikuyu');
INSERT INTO `Towns` (Id,Name) VALUES (11,'Kangundo-Tala');
INSERT INTO `Towns` (Id,Name) VALUES (12,'Malindi');
INSERT INTO `Towns` (Id,Name) VALUES (13,'Naivasha');
INSERT INTO `Towns` (Id,Name) VALUES (14,'Kitui');
INSERT INTO `Towns` (Id,Name) VALUES (15,'Machakos');
INSERT INTO `Towns` (Id,Name) VALUES (16,'Thika');
INSERT INTO `Towns` (Id,Name) VALUES (17,'Athi River (Mavoko)');
INSERT INTO `Towns` (Id,Name) VALUES (18,'Karuri');
INSERT INTO `Towns` (Id,Name) VALUES (19,'Nyeri');
INSERT INTO `Towns` (Id,Name) VALUES (20,'Kilifi');
INSERT INTO `Towns` (Id,Name) VALUES (21,'Garissa');
INSERT INTO `Towns` (Id,Name) VALUES (22,'Vihiga');
INSERT INTO `Towns` (Id,Name) VALUES (23,'Mumias');
INSERT INTO `Towns` (Id,Name) VALUES (24,'Bomet');
INSERT INTO `Towns` (Id,Name) VALUES (25,'Molo');
INSERT INTO `Towns` (Id,Name) VALUES (26,'Ngong');
INSERT INTO `Towns` (Id,Name) VALUES (27,'Kitale');
INSERT INTO `Towns` (Id,Name) VALUES (28,'Litein');
INSERT INTO `Towns` (Id,Name) VALUES (29,'Limuru');
INSERT INTO `Towns` (Id,Name) VALUES (30,'Kericho');
INSERT INTO `Towns` (Id,Name) VALUES (31,'Kimilili');
INSERT INTO `Towns` (Id,Name) VALUES (32,'Awasi');
INSERT INTO `Towns` (Id,Name) VALUES (33,'Kakamega');
INSERT INTO `Towns` (Id,Name) VALUES (34,'Kapsabet');
INSERT INTO `Towns` (Id,Name) VALUES (35,'Mariakani');
INSERT INTO `Towns` (Id,Name) VALUES (36,'Kiambu');
INSERT INTO `Towns` (Id,Name) VALUES (37,'Mandera');
INSERT INTO `Towns` (Id,Name) VALUES (38,'Nyamira');
INSERT INTO `Towns` (Id,Name) VALUES (39,'Mwingi');
INSERT INTO `Towns` (Id,Name) VALUES (40,'Kisii');
INSERT INTO `Towns` (Id,Name) VALUES (41,'Wajir');
INSERT INTO `Towns` (Id,Name) VALUES (42,'Rongo');
INSERT INTO `Towns` (Id,Name) VALUES (43,'Bungoma');
INSERT INTO `Towns` (Id,Name) VALUES (44,'Ahero');
INSERT INTO `Towns` (Id,Name) VALUES (45,'Nandi Hills');
INSERT INTO `Towns` (Id,Name) VALUES (46,'Makuyu');
INSERT INTO `Towns` (Id,Name) VALUES (47,'Kapenguria');
INSERT INTO `Towns` (Id,Name) VALUES (48,'Taveta');
INSERT INTO `Towns` (Id,Name) VALUES (49,'Narok');
INSERT INTO `Towns` (Id,Name) VALUES (50,'Ol Kalou');
INSERT INTO `Towns` (Id,Name) VALUES (51,'Kakuma');
INSERT INTO `Towns` (Id,Name) VALUES (52,'Webuye');
INSERT INTO `Towns` (Id,Name) VALUES (53,'Malaba');
INSERT INTO `Towns` (Id,Name) VALUES (54,'Mbita Point');
INSERT INTO `Towns` (Id,Name) VALUES (55,'Ukunda');
INSERT INTO `Towns` (Id,Name) VALUES (56,'Wundanyi');
INSERT INTO `Towns` (Id,Name) VALUES (57,'Busia');
INSERT INTO `Towns` (Id,Name) VALUES (58,'Runyenjes');
INSERT INTO `Towns` (Id,Name) VALUES (59,'Migori');
INSERT INTO `Towns` (Id,Name) VALUES (60,'Malava');
INSERT INTO `Towns` (Id,Name) VALUES (61,'Suneka');
INSERT INTO `Towns` (Id,Name) VALUES (62,'Embu');
INSERT INTO `Towns` (Id,Name) VALUES (63,'Ogembo');
INSERT INTO `Towns` (Id,Name) VALUES (64,'Homa Bay');
INSERT INTO `Towns` (Id,Name) VALUES (65,'Lodwar');
INSERT INTO `Towns` (Id,Name) VALUES (66,'Kitengela');
INSERT INTO `Towns` (Id,Name) VALUES (67,'Ukwala');
INSERT INTO `Towns` (Id,Name) VALUES (68,'Keroka');
INSERT INTO `Towns` (Id,Name) VALUES (69,'Meru');
INSERT INTO `Towns` (Id,Name) VALUES (70,'Matuu');
INSERT INTO `Towns` (Id,Name) VALUES (71,'Oyugis');
INSERT INTO `Towns` (Id,Name) VALUES (72,'Nyahururu');
INSERT INTO `Towns` (Id,Name) VALUES (73,'Kipkelion');
INSERT INTO `Towns` (Id,Name) VALUES (74,'Luanda');
INSERT INTO `Towns` (Id,Name) VALUES (75,'Nanyuki');
INSERT INTO `Towns` (Id,Name) VALUES (76,'Maua');
INSERT INTO `Towns` (Id,Name) VALUES (77,'Mtwapa');
INSERT INTO `Towns` (Id,Name) VALUES (78,'Isiolo');
INSERT INTO `Towns` (Id,Name) VALUES (79,'Eldama Ravine');
INSERT INTO `Towns` (Id,Name) VALUES (80,'Voi');
INSERT INTO `Towns` (Id,Name) VALUES (81,'Siaya');
INSERT INTO `Towns` (Id,Name) VALUES (82,'Nyansiongo');
INSERT INTO `Towns` (Id,Name) VALUES (83,'Londiani');
INSERT INTO `Towns` (Id,Name) VALUES (84,'Iten/Tambach');
INSERT INTO `Towns` (Id,Name) VALUES (85,'Chuka');
INSERT INTO `Towns` (Id,Name) VALUES (86,'Malakisi');
INSERT INTO `Towns` (Id,Name) VALUES (87,'Juja');
INSERT INTO `Towns` (Id,Name) VALUES (88,'Ongata Rongai');
INSERT INTO `Towns` (Id,Name) VALUES (89,'Bondo');
INSERT INTO `Towns` (Id,Name) VALUES (90,'Moyale');
INSERT INTO `Towns` (Id,Name) VALUES (91,'Maralal');
INSERT INTO `Towns` (Id,Name) VALUES (92,'Gilgil');
INSERT INTO `Towns` (Id,Name) VALUES (93,'Nambale');
INSERT INTO `Towns` (Id,Name) VALUES (94,'Tabaka');
INSERT INTO `Towns` (Id,Name) VALUES (95,'Muhoroni');
INSERT INTO `Towns` (Id,Name) VALUES (96,'Kerugoya/Kutus');
INSERT INTO `Towns` (Id,Name) VALUES (97,'Ugunja');
INSERT INTO `Towns` (Id,Name) VALUES (98,'Yala');
INSERT INTO `Towns` (Id,Name) VALUES (99,'Rumuruti');
INSERT INTO `Towns` (Id,Name) VALUES (100,'Burnt Forest');
INSERT INTO `Towns` (Id,Name) VALUES (101,'Maragua');
INSERT INTO `Towns` (Id,Name) VALUES (102,'Kendu Bay');
INSERT INTO `Towns` (Id,Name) VALUES (103,'Archerspost');
INSERT INTO `Towns` (Id,Name) VALUES (104,'Bahati');
INSERT INTO `Towns` (Id,Name) VALUES (105,'Baragoi');
INSERT INTO `Towns` (Id,Name) VALUES (106,'Baringo');
INSERT INTO `Towns` (Id,Name) VALUES (107,'Bissil');
INSERT INTO `Towns` (Id,Name) VALUES (108,'Bumala');
INSERT INTO `Towns` (Id,Name) VALUES (109,'Butere');
INSERT INTO `Towns` (Id,Name) VALUES (110,'Cheptais');
INSERT INTO `Towns` (Id,Name) VALUES (111,'Chogoria');
INSERT INTO `Towns` (Id,Name) VALUES (112,'Daadab');
INSERT INTO `Towns` (Id,Name) VALUES (113,'Dundori');
INSERT INTO `Towns` (Id,Name) VALUES (114,'Endarasha');
INSERT INTO `Towns` (Id,Name) VALUES (115,'Funyula');
INSERT INTO `Towns` (Id,Name) VALUES (116,'Garbatula');
INSERT INTO `Towns` (Id,Name) VALUES (117,'Gatundu');
INSERT INTO `Towns` (Id,Name) VALUES (118,'Hola');
INSERT INTO `Towns` (Id,Name) VALUES (119,'Iten');
INSERT INTO `Towns` (Id,Name) VALUES (120,'Juakali');
INSERT INTO `Towns` (Id,Name) VALUES (121,'Kabarnet');
INSERT INTO `Towns` (Id,Name) VALUES (122,'Kabati');
INSERT INTO `Towns` (Id,Name) VALUES (123,'Kabuti');
INSERT INTO `Towns` (Id,Name) VALUES (124,'Kagio');
INSERT INTO `Towns` (Id,Name) VALUES (125,'Kagumo');
INSERT INTO `Towns` (Id,Name) VALUES (126,'Kajiado');
INSERT INTO `Towns` (Id,Name) VALUES (127,'Kaloleni');
INSERT INTO `Towns` (Id,Name) VALUES (128,'Kandara');
INSERT INTO `Towns` (Id,Name) VALUES (129,'Kangari');
INSERT INTO `Towns` (Id,Name) VALUES (130,'Kangema');
INSERT INTO `Towns` (Id,Name) VALUES (131,'Kangundo');
INSERT INTO `Towns` (Id,Name) VALUES (132,'Kapcherop');
INSERT INTO `Towns` (Id,Name) VALUES (133,'Kapsokwony');
INSERT INTO `Towns` (Id,Name) VALUES (134,'Kapsowar');
INSERT INTO `Towns` (Id,Name) VALUES (135,'Karatina');
INSERT INTO `Towns` (Id,Name) VALUES (136,'Kathiani');
INSERT INTO `Towns` (Id,Name) VALUES (137,'Kibwezi');
INSERT INTO `Towns` (Id,Name) VALUES (138,'Kinamba');
INSERT INTO `Towns` (Id,Name) VALUES (139,'Kinna');
INSERT INTO `Towns` (Id,Name) VALUES (140,'Kiria-Ini');
INSERT INTO `Towns` (Id,Name) VALUES (141,'Laisamis');
INSERT INTO `Towns` (Id,Name) VALUES (142,'Lamu');
INSERT INTO `Towns` (Id,Name) VALUES (143,'Lare');
INSERT INTO `Towns` (Id,Name) VALUES (144,'Loiyangalani');
INSERT INTO `Towns` (Id,Name) VALUES (145,'Lokichogio');
INSERT INTO `Towns` (Id,Name) VALUES (146,'Lolgorian');
INSERT INTO `Towns` (Id,Name) VALUES (147,'Lungalunga');
INSERT INTO `Towns` (Id,Name) VALUES (148,'Machinery');
INSERT INTO `Towns` (Id,Name) VALUES (149,'Magarini');
INSERT INTO `Towns` (Id,Name) VALUES (150,'Majimazuri');
INSERT INTO `Towns` (Id,Name) VALUES (151,'Marereni');
INSERT INTO `Towns` (Id,Name) VALUES (152,'Maseno');
INSERT INTO `Towns` (Id,Name) VALUES (153,'Masii');
INSERT INTO `Towns` (Id,Name) VALUES (154,'Maunarok');
INSERT INTO `Towns` (Id,Name) VALUES (155,'Mazeras');
INSERT INTO `Towns` (Id,Name) VALUES (156,'Merti');
INSERT INTO `Towns` (Id,Name) VALUES (157,'Mitunguu');
INSERT INTO `Towns` (Id,Name) VALUES (158,'Mogonga');
INSERT INTO `Towns` (Id,Name) VALUES (159,'Mogotio');
INSERT INTO `Towns` (Id,Name) VALUES (160,'Mtito Andei');
INSERT INTO `Towns` (Id,Name) VALUES (161,'Muhuru Bay');
INSERT INTO `Towns` (Id,Name) VALUES (162,'Muranga');
INSERT INTO `Towns` (Id,Name) VALUES (163,'Mwatate');
INSERT INTO `Towns` (Id,Name) VALUES (164,'Mweiga');
INSERT INTO `Towns` (Id,Name) VALUES (165,'Nairagieenkare');
INSERT INTO `Towns` (Id,Name) VALUES (166,'Naromoru');
INSERT INTO `Towns` (Id,Name) VALUES (167,'Ndori');
INSERT INTO `Towns` (Id,Name) VALUES (168,'Ngong’');
INSERT INTO `Towns` (Id,Name) VALUES (169,'Njabini');
INSERT INTO `Towns` (Id,Name) VALUES (170,'Nyakach');
INSERT INTO `Towns` (Id,Name) VALUES (171,'Nyamache');
INSERT INTO `Towns` (Id,Name) VALUES (172,'Nyamarambe');
INSERT INTO `Towns` (Id,Name) VALUES (173,'Olenguruone');
INSERT INTO `Towns` (Id,Name) VALUES (174,'Othaya');
INSERT INTO `Towns` (Id,Name) VALUES (175,'Rongai');
INSERT INTO `Towns` (Id,Name) VALUES (176,'Salgaa');
INSERT INTO `Towns` (Id,Name) VALUES (177,'Siakago');
INSERT INTO `Towns` (Id,Name) VALUES (178,'Sindo');
INSERT INTO `Towns` (Id,Name) VALUES (179,'Sololo');
INSERT INTO `Towns` (Id,Name) VALUES (180,'Sultan Hamud');
INSERT INTO `Towns` (Id,Name) VALUES (181,'Timboroa');
INSERT INTO `Towns` (Id,Name) VALUES (182,'Tongaren');
INSERT INTO `Towns` (Id,Name) VALUES (183,'Watamu');
INSERT INTO `Towns` (Id,Name) VALUES (184,'Wote');
INSERT INTO `Towns` (Id,Name) VALUES (185,'Mangu');
CREATE TABLE `Speakers` (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Name`	TEXT NOT NULL UNIQUE
);
INSERT INTO `Speakers` (Id,Name) VALUES (0,'_default_');
INSERT INTO `Speakers` (Id,Name) VALUES (1,'Wilbur Omae');
INSERT INTO `Speakers` (Id,Name) VALUES (2,'Webster Otugah');
CREATE TABLE "Sermons" (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Series`	INTEGER NOT NULL DEFAULT 0,
	`DateCreated`	TEXT NOT NULL,
	`Venue`	INTEGER NOT NULL DEFAULT 0,
	`Town`	INTEGER NOT NULL DEFAULT 0,
	`Activity`	INTEGER NOT NULL DEFAULT 0,
	`Speaker`	INTEGER NOT NULL DEFAULT 0,
	`Title`	TEXT NOT NULL,
	`Text`	TEXT,
	`Hymn`	TEXT,
	`Content`	TEXT NOT NULL,
	FOREIGN KEY(`Series`) REFERENCES `Series`(`Id`),
	FOREIGN KEY(`Venue`) REFERENCES `Venues`(`Id`),
	FOREIGN KEY(`Town`) REFERENCES `Towns`(`Id`),
	FOREIGN KEY(`Activity`) REFERENCES `Activities`(`Id`),
	FOREIGN KEY(`Speaker`) REFERENCES `Speakers`(`Id`)
);
INSERT INTO `Sermons` (Id,Series,DateCreated,Venue,Town,Activity,Speaker,Title,Text,Hymn,Content) VALUES (1,0,'08-Jul-17 21:10:00',2,185,2,2,'Let Jesus Walk among the Sycamore Trees','Luke 19:4','309','{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Times New Roman;}}
\viewkind4\uc1\pard\f0\fs41 Christ was on a mission.\par
}
');
CREATE TABLE `SeriesSpeakers` (
	`SeriesId`	INTEGER NOT NULL,
	`SpeakerId`	INTEGER NOT NULL,
	PRIMARY KEY(`SeriesId`,`SpeakerId`),
	FOREIGN KEY(`SeriesId`) REFERENCES Series(Id),
	FOREIGN KEY(`SpeakerId`) REFERENCES Speakers(Id)
);
INSERT INTO `SeriesSpeakers` (SeriesId,SpeakerId) VALUES (1,1);
INSERT INTO `SeriesSpeakers` (SeriesId,SpeakerId) VALUES (2,2);
CREATE TABLE "Series" (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Theme`	TEXT NOT NULL,
	`Venue`	INTEGER NOT NULL DEFAULT 0,
	`Town`	INTEGER NOT NULL DEFAULT 0,
	`Activity`	INTEGER NOT NULL DEFAULT 0,
	`StartDate`	TEXT NOT NULL,
	`EndDate`	TEXT NOT NULL,
	FOREIGN KEY(`Venue`) REFERENCES `Venues`(`Id`),
	FOREIGN KEY(`Town`) REFERENCES `Towns`(`Id`),
	FOREIGN KEY(`Activity`) REFERENCES `Activities`(`Id`)
);
INSERT INTO `Series` (Id,Theme,Venue,Town,Activity,StartDate,EndDate) VALUES (0,'',0,0,0,'','');
INSERT INTO `Series` (Id,Theme,Venue,Town,Activity,StartDate,EndDate) VALUES (1,'Hard Questions',1,87,1,'07-Jul-17 17:13:38','07-Jul-17 17:13:38');
INSERT INTO `Series` (Id,Theme,Venue,Town,Activity,StartDate,EndDate) VALUES (2,'Unleavened Bread',2,87,5,'08-Jul-17 20:52:20','08-Jul-17 20:52:20');
CREATE TABLE `RODs` (
	`DocId`	INTEGER NOT NULL,
	`DocTitle`	TEXT,
	PRIMARY KEY(`DocId`),
	FOREIGN KEY(`DocId`) REFERENCES Sermons(Id),
	FOREIGN KEY(`DocTitle`) REFERENCES Sermons(Title)
);
CREATE TABLE "Preferences" (
	`PrinterName`	TEXT,
	`PrinterScheme`	TEXT DEFAULT 'White/Black',
	`ColourFont`	TEXT DEFAULT 'AACCFF',
	`ColourControls`	TEXT DEFAULT 222233,
	`FontSystem`	TEXT DEFAULT 'Times New Roman',
	`FontReader`	TEXT DEFAULT 'Times New Roman',
	`FontWriter`	TEXT DEFAULT 'Times New Roman',
	`RODMaxNumber`	INTEGER DEFAULT 10,
	`SortingFilter`	TEXT DEFAULT 'SPEAKER',
	`ShowWelcomeScreen`	INTEGER DEFAULT 0
);
CREATE TABLE `Activities` (
	`Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`Name`	TEXT NOT NULL UNIQUE
);
INSERT INTO `Activities` (Id,Name) VALUES (0,'_default_');
INSERT INTO `Activities` (Id,Name) VALUES (1,'Bible Study');
INSERT INTO `Activities` (Id,Name) VALUES (2,'Sermon');
INSERT INTO `Activities` (Id,Name) VALUES (3,'Sermonette');
INSERT INTO `Activities` (Id,Name) VALUES (4,'Sharing');
INSERT INTO `Activities` (Id,Name) VALUES (5,'Sabbath Service');
COMMIT;
