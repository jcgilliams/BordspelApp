-- Verwijderen bestaande tabellen + relaties + constraints + schema
---- tabellen verwijderen
IF OBJECT_ID('ProjectBGG.BordspelPersoon','U')IS NOT NULL
	DROP TABLE ProjectBGG.BordspelPersoon;
IF OBJECT_ID('ProjectBGG.BordspelUitgeverij','U')IS NOT NULL
	DROP TABLE ProjectBGG.BordspelUitgeverij;
IF OBJECT_ID('ProjectBGG.BordspelGebruiker','U')IS NOT NULL
	DROP TABLE ProjectBGG.BordspelGebruiker;
IF OBJECT_ID('ProjectBGG.Uitgeverij','U')IS NOT NULL
	DROP TABLE ProjectBGG.Uitgeverij;
IF OBJECT_ID('ProjectBGG.Persoon','U')IS NOT NULL
	DROP TABLE ProjectBGG.Persoon;
IF OBJECT_ID('ProjectBGG.Gebruiker','U')IS NOT NULL
	DROP TABLE ProjectBGG.Gebruiker;
IF OBJECT_ID('ProjectBGG.Rol','U')IS NOT NULL
	DROP TABLE ProjectBGG.Rol;
IF OBJECT_ID('ProjectBGG.Bordspel','U')IS NOT NULL
	DROP TABLE ProjectBGG.Bordspel;

---- schema verwijderen
DROP SCHEMA IF EXISTS ProjectBGG;
GO

-- Creatie schema
CREATE SCHEMA ProjectBGG;
GO

-- Tabellen van BGG
	-- Creëren tabellen
CREATE TABLE ProjectBGG.Bordspel
(
    id   int IDENTITY(35000,1),
    naam varchar(45) NOT NULL,
	jaar int NOT NULL,
	beschrijving varchar(max),
	minAantalSpelers int,
	maxAantalSpelers int,
	minSpeeltijd int,
	maxSpeeltijd int,
	leeftijd int,
    CONSTRAINT PK_Bordspel 
		PRIMARY KEY (id)
);

CREATE TABLE ProjectBGG.Rol
(
	id   int IDENTITY (1,1),
	beschrijving varchar(max) NOT NULL,
	CONSTRAINT PK_Rol 
		PRIMARY KEY (id)
);

CREATE TABLE ProjectBGG.Gebruiker
(
	id   int IDENTITY (1,1),
	username varchar(45) NOT NULL,
	pasword varchar(45) NOT NULL,
	CONSTRAINT PK_Gebruiker 
		PRIMARY KEY (id)
);

CREATE TABLE ProjectBGG.Persoon
(
	id   int IDENTITY (1,1),
	naam varchar(45) NOT NULL,
	voornaam varchar(45) NOT NULL,
	beschrijving varchar(max),
	gebruikerId int NULL,
	CONSTRAINT PK_Persoon 
		PRIMARY KEY (id),
	CONSTRAINT FK_Persoon_Gebruiker
        FOREIGN KEY (gebruikerId)
            REFERENCES ProjectBGG.Gebruiker (id)
			ON DELETE SET NULL
);

CREATE TABLE ProjectBGG.Uitgeverij
(
	id   int IDENTITY (1,1),
	naam varchar(45) NOT NULL,
	beschrijving varchar(max),
	website varchar(45),
	persoonId int NULL,
	land varchar(45) NOT NULL,
	CONSTRAINT PK_Uitgeverij 
		PRIMARY KEY (id),
	CONSTRAINT FK_Uitgeverij_Persoon
        FOREIGN KEY (persoonId)
            REFERENCES ProjectBGG.Persoon (id)
			ON DELETE SET NULL
);

CREATE TABLE ProjectBGG.BordspelGebruiker
(
	id   int IDENTITY (1,1),
	bordspelId int NOT NULL,
	gebruikerId int NOT NULL,
	rating int,
	CONSTRAINT PK_BordspelGebruiker 
		PRIMARY KEY (id),
	CONSTRAINT FK_BordspelGebruiker_Bordspel
        FOREIGN KEY (bordspelId)
            REFERENCES ProjectBGG.Bordspel (id),
	CONSTRAINT FK_BordspelGebruiker_Gebruiker
        FOREIGN KEY (gebruikerId)
            REFERENCES ProjectBGG.Gebruiker (id)
);

CREATE TABLE ProjectBGG.BordspelUitgeverij
(
	id   int IDENTITY (1,1),
	bordspelId int NOT NULL,
	uitgeverijId int NOT NULL,
	taal varchar(45),
	CONSTRAINT PK_BordspelUitgeverij 
		PRIMARY KEY (id),
	CONSTRAINT FK_BordspelUitgeverij_Bordspel
        FOREIGN KEY (bordspelId)
            REFERENCES ProjectBGG.Bordspel (id),
	CONSTRAINT FK_BordspelUitgeverij_Uitgeverij
        FOREIGN KEY (uitgeverijId)
            REFERENCES ProjectBGG.Uitgeverij (id)
);

CREATE TABLE ProjectBGG.BordspelPersoon
(
	id   int IDENTITY (1,1),
	bordspelId int NOT NULL,
	persoonId int NOT NULL,
	rolId int NOT NULL,
	CONSTRAINT PK_BordspelPersoon 
		PRIMARY KEY (id),
	CONSTRAINT FK_BordspelPersoon_Bordspel
        FOREIGN KEY (bordspelId)
            REFERENCES ProjectBGG.Bordspel (id),
	CONSTRAINT FK_BordspelPersoon_Persoon
        FOREIGN KEY (persoonId)
            REFERENCES ProjectBGG.Persoon (id),
	CONSTRAINT FK_BordspelPersoon_Rol
        FOREIGN KEY (rolId)
            REFERENCES ProjectBGG.Rol (id)
);

-- Vullen databank

SET IDENTITY_INSERT ProjectBGG.Bordspel ON;

INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (173346,'7 Wonders Duel',2015,'Onthul een opwindende en oude wereld met 7 Wonders Duel, een strategisch spel voor 2 spelers. Leid jouw beschaving naar weelde en bouw prestigieuze gebouwen en wonderen!',2,2,30,30,10);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (20080,'Aqua Romana',2005,'Water is van levensbelang, iets dat ze in het Romeinse tijdperk meer dan ooit beseften. Daarom bouwden de Romeinen de welgekende aquaducten. In Aqua Romana proberen de spelers zo lang mogelijke aquaducten te bouwen. Het bouwen is een moeilijke aangelegenheid waarbij de regels van de bouwmeesters strikt gevolgd moeten worden. Helaas lopen ook de andere spelers vaak in de weg.',2,4,45,60,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (236457,'Architecten van het Westelijk Koninkrijk',2018,'Architecten van het Westelijk Koninkrijk speelt zich af rond het jaar 850, tegen het einde van het Karolingische rijk. De spelers zijn architecten die indruk proberen te maken op de koning. Ze proberen hun aanzien te behouden door diverse gebouwen in door hem veroverde gebieden te bouwen. De spelers moeten grondstoffen verzamelen, leerlingen inhuren en een scherpe blik op hun mankracht houden. Het zijn verraderlijke tijden en de concurrentie zal niet stoppen om jouw proces te vertragen. Blijf je eervol of begeef je jezelf in gezelschap van dieven en handelaren op de zwarte markt?',1,5,60,80,12);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (230802,'Azul',2017,'Azul is een tactisch spel waarin de spelers de opdracht krijgen van de koning om de muren van het koninklijk paleis in Evora te verfraaien. Je moet patronen vormen en de muren van het paleis bekleden. Scoor de meeste punten om te winnen!',2,4,30,45,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (11,'Bohnanza: Jubiläumsedition',1997,'In dit communicatieve kaartspel handelen de spelers met hun bonen en verbouwen deze op hun akkers. Wie troggelt de andere spelers de beste bonen af en verdient het meeste geld?',2,7,45,45,13);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (822,'Carcassonne',2000,'Rond Carcassonne ontwikkelt zich een landschap, waar de spelers hun meeples inzetten om zoveel mogelijk punten te halen. Meeples scoren als een bezette weg, stad, klooster of weiland wordt afgebouwd. Zet ze echter niet te snel in, want je hebt er maar weinig… Bovendien kunnen anderen jou dwarsbomen!',2,5,30,45,7);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (245476,'CuBirds',2018,'In dit snelle en opvallende kaartspel moeten spelers vogels verzamelen. Wie als eerst 7 verschillende vogels heeft, wint. Elke beurt mag een speler alle vogels uit zijn hand aanleggen bij een van de 4 rijtjes vogels op tafel. Vogels die hij zo insluit mag de speler vervolgens weer op hand nemen. Zo proberen spelers genoeg vogels van een soort op hand te krijgen om er 1 of 2 van in hun verzameling te mogen doen.',2,5,20,20,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (15512,'Diamant',2005,'Met voorzichtige stappen en het licht van je fakkel, probeer je zo de Tacora Grot te ontdekken. Iedere ronde ga je dieper de grot in en zul je meer nieuwe kamers gaan zien, met daarin diamanten die je kunt verzamelen. Na het bezoeken van een kamer heb je twee keuzes: 1; je verlaat de grot en neemt al je schatten mee. 2; je gaat dieper de grot in op zoek naar nog meer diamanten. Zodra je besluit om verder te gaan en je komt een trap-card tegen, word je direct naar de uitgang verplaatst en verlies je al je kostbaarheden.',3,8,30,30,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (39856,'Dixit',2008,'Bij dit gezellige bordspel is het belangrijk om je medespelers goed in te schatten en je verbeelding de vrije loop te laten gaan. Raad jij de kaart van de verteller? Het sprookjesachtige spel Dixit bevat prachtige kaarten die voor interpretatie vatbaar zijn. Het spel heeft weinig spelregels, waardoor het heel snel te leren is en geschikt is voor kinderen vanaf 8 jaar.',3,6,30,30,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (929,'De Grote Dalmuti',1995,'Het leven is niet eerlijk ... en dat geldt ook voor De Grote Dalmuti! In de ene beurt kan je de koning zijn, in de volgende weer een bediende. Strijd voor de hoogste sociale klasse en kroon jezelf tot De Grote Dalmuti!',4,8,60,60,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (466,'Inkognito',1988,'Inkognito is een uniek spel van spionage en bedrog voor drie of vier spelers die de rol aannemen van geheim agenten tijdens een Venetiaans Carnaval, die hen in staat stelt om maskers te dragen als ze op zoek gaan naar geheime informatie van de stad.',3,5,90,90,10);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (8098,'Jungle Speed',1997,'Jungle Speed is dé ultieme test om te zien wie het beste kan observeren en vliegensvlug kan reageren. De speler die als eerste al hun kaarten heeft weggespeeld, wint het spel!',2,8,10,10,7);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (1117,'Koehandel',1985,'Éénmaal... andermaal... verkocht! Wie is de gelukkige eigenaar van de prijskoe Bertha of één van de andere dieren? Menige doortrapte koehandel loopt volkomen uit de hand. Maar wie heeft nu eigenlijk wie te pakken? Alleen wie met boerenverstand biedt en listiger is dan de rest krijgt de lucratiefste dieren in handen!',3,5,45,45,10);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (9440,'Maharaja',2004,'Maharadja is verrassend leuk en bovendien een zeer toegankelijk en slim bordspel voor een breed publiek, waarin je de prachtigste huizen en paleizen bouwt uit naam van de Maharadja. Als de Maharadja roept, voer jij als belangrijke prins zijn orders uit en reist door heel India om paleizen en huizen voor hem te bouwen. De Maharadja beloont de prins met de meeste gebouwen in de stad waar hij langsreist, met veel geld. Dit geld kun je gebruiken om, jawel, paleizen in de volgende stad te bouwen...',2,5,90,90,12);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (30549,'Pandemic',2008,'Pandemic is een coöperatief bordspel, waarbij je niet tegen elkaar, maar met elkaar speelt met één gezamenlijk doel: de mensheid redden. Vier ziektes bedreigen het voortbestaan van de wereld. Aan jullie de belangrijke taak om de wereld te redden.',2,4,45,45,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (3076,'Puerto Rico',2002,'In 1493 ontdekt Columbus Puerto Rico, de parel van het Caribische gebied. En al een goede halve eeuw later beleeft het eiland haar eerste grote bloeiperiode. De spelers kruipen tijdens het spel steeds weer in een andere rol. Als kolonist leggen ze vruchtbare plantages aan. Als opzichter produceren ze waardevolle goederen, die ze dan, in de rol van handelaar, met de hoogst mogelijke winst verkopen. Dat verdiende geld gebruiken ze dan weer om als bouwmeester indrukwekkende stadsgebouwen op te richten.',3,5,90,150,12);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (339484,'Savannah Park',2021,'In Savannah Park heb je je eigen wildlife park en probeer je die zo goed mogelijk in te delen. Gelukkig zijn de regels daarvoor heel makkelijk, want als je aan de beurt bent noem je een tegel en vervolgens moet iedere speler die tegel verplaatsen op zijn of haar spelersbordje. Daarmee probeer je groepen dieren te maken, maar pas op voor het vuur. Als je namelijk de verkeerde dieren naast het vuur zet, dan raak je ze kwijt en tellen ze niet mee aan het einde van het spel. Weet jij je Savannah Park zo slim mogelijk in te delen?',1,4,20,40,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (202670,'Ticket to Ride: Rails & Sails',2016,'Speciale versie van Ticket to Ride waarbij je niet alleen per trein, maar ook per boot reist om de hele wereld te ontdekken! Verzamel de juiste trein- en schipkaarten om verbindingen over land en zee aan te leggen. Hier scoor je punten voor en je kunt nog meer punten verdienen als je complete routes weet aan te leggen. Als je jouw routes echter niet afmaakt, krijg je strafpunten aan het einde van het spel!',2,5,60,120,10);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (283864,'Trails Of Tucana',2019,'Teken routes op je eiland en scoor punten door dorpen en bezienswaardigheden met elkaar te verbinden. Niet elk gebied is even makkelijk te doorkruisen, dus plan je routes zorgvuldig! Elk spel worden de dorpen willekeurig geplaatst, wat telkens een nieuwe uitdagende puzzel oplevert.',1,8,15,15,8);
INSERT INTO [ProjectBGG].[Bordspel] (id, naam, jaar, beschrijving, minAantalSpelers, maxAantalSpelers, minSpeeltijd, maxSpeeltijd, leeftijd)
	VALUES (266192,'Wingspan',2019,'Wingspan is een tactisch spel met een prachtige vormgeving. Door vogels in je persoonlijke natuurgebied uit te zetten verzamel je voedsel, eieren en punten. Wie zijn vogels het slimst samen laat werken, wint het spel. ',1,5,40,70,10);

SET IDENTITY_INSERT ProjectBGG.Bordspel OFF;


INSERT INTO [ProjectBGG].[Rol] VALUES ('Designer');
INSERT INTO [ProjectBGG].[Rol] VALUES ('Artist');

INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('jc','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('lien','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('toinito','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Martin S','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Michael Menzel','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('shem84','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('themico','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('BigdaddyTR','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Repos Production','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Nekrataal','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('BohnanZar','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Mavericius','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('AlexYeager','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Hans im Glueck','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('UltraPRO','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('s2alexan','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('kherubim','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('faidutti','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('lalloq','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('marika','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Phelddagrif','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('leocolo','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('UniversalHead','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('biohazard','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('tom vuarchex','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('GrabTiger','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('ulible','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('DjihEf','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('mleacock','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('AS-Leo','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('vittorioso','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Adrien','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('alanrmoon','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('CyrilleD','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('jimmythesaint82','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('Esven','12345');
INSERT INTO [ProjectBGG].[Gebruiker] VALUES ('elizharg','12345');

INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Bauza','Antoine','Antoine Bauza (born 1978 )is the popular designer of several award-winning games, including 7 Wonders and Hanabi (which won the 2013 Spiel des Jahres prize). Even before the success of 7 Wonders, Antoine was well-known for his many other games, including Ghost Stories and Tokaido.',3);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Coimbra','Miguel','Miguel Coimbra (born December 9, 1977) is a French freelance artist and graphic artist of Portuguese origin.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Schlegel','Martin',NULL,4);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Menzel','Michael','Michael Menzel was born in 1974. He lives with his wife Stefanie and his son Johannes in Krefeld. In 2003 he had a lucky strike when he started illustrating games. Up until 2007 he had mainly illustrated family games and games for adults. HABA in 2007, however, offered him the possibility to illustrate a children game and ever since that wonderful and inspiring cooperation, working on children games has become one of his favorite activities. In the last years he has already inspired various HABA-games with his great illustrations, such as Raggedy Rat Rascal, Captn Look and Wild Vikings.',5);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Philips','Shem','Shem Phillips is a board game designer from New Zealand. He founded Garphill Games in 2009 and published a wide variety of games since. He is most well known for his medium to medium-heavy Euros such as his line of medieval trilogies (North Sea, West Kingdom, South Tigris & East ???) and Ancient Anthology series. He often works with S. J. Macdonald.',6);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Dimitrievski','Mihajlo','Mihajlo Dimitrievski is a board game artist/illustrator from Bitola, North Macedonia. His nickname is "The Mico". He often collaborates with Garphill Games, Final Frontier Games and Daily Magic Games.',7);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Queen Games','Uitgeverij',NULL,8);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Repos Production','Uitgeverij',NULL,9);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('White Goblin Games','Uitgeverij',NULL,10);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Next Move Games','Uitgeverij',NULL,NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Kiesling','Michael','Michael Kiesling (born 1957 in Bremen, Germany) is a game designer who frequently collaborates with game designer Wolfgang Kramer. He is also a manager of a software company in Bremen. For many years, Kramer and Michael Kiesling worked on games together by telephone and fax correspondence despite never having met.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Quilliams','Chris','Chris Quilliams is an artist from Winnipeg, Manitoba, Canada, where he has lived most of his life. He studied fine arts at the University of Manitoba. He now lives in Rigaud, Quebec, Canada working in studio for Plan B games.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Rosenberg','Uwe','Uwe Rosenberg (born 27 March 1970 in Aurich, Germany) is a German game designer. He has become known mainly for his card game Bohnanza, which is successful both in Germany as internationally. He also designed Agricola, a game that dethroned Puerto Rico as the highest rated game on BoardGameGeek.com.',11);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Klemens','Franz','Klemens Franz (born April, 17 1979 in Graz, Austria) is a game artist from Austria.',12);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Amigo','Uitgeverij',NULL,13);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Hans im Glueck','Uitgeverij',NULL,14);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Wrede','Klaus-Jürgen','Klaus-Jürgen Wrede (born 1963 in Meschede, North Rhine-Westphalia, Germany) is a board game designer and music and theology teacher from Arnsberg, North Rhine-Westphalia, Germany, and the city of Cologne. He created the popular game Carcassonne and helped create most of the other Carcassonne games.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Matthäus','Doris','Doris Matthäus (born April 3, 1963 in Erlangen, Bavaria, Germany) is an artist who was perhaps the first German games illustrator to receive international recognition. She is also well-known for illustrating the popular game Carcassonne.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('UltraPro','Uitgeverij',NULL,15);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Alexander','Stefan','Stefan Alexander is a Canadian board game designer.',16);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('der Nederlanden','Kristiaan','Kristiaan is a Dutch illustrator, living in Amsterdam. He graduated in Media and Communication Studies at the University of Amsterdam and he has been working since 2002 as a freelance illustrator. The first board game he illustrated is CuBirds.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('IELLO','Uitgeverij',NULL,17);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Faidutti','Bruno','Bruno Faidutti (born October 23, 1961) is a prolific French game designer, historian, and sociologist who frequently collaborates with other game designers, including Alan R. Moon, Bruno Cathala, and Michael Schacht, although he has also done many designs on his own, including perhaps his most famous game, Citadels. Faidutti is a contributor to BGG, under the username faidutti.',18);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Asselborn','Jörg','Jörg Asselborn, born in 1972, studied design and specialized in illustration. Today, he lives with his family in Wiesbaden (Germany) and works for a design studio. He is without a doubt a most talented watercolor and urban-sketch artist who dares sometimes to refer to himself as an illustrator.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Libellud','Uitgeverij',NULL,19);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Roubira','Jean-Louis','Jean-Louis Roubira is a French child psychologist and games author who especially for 2010 with the Jury Prize Game of the Year winning card and guessing game Dixit was known.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Cardouat','Marie','Marie Cardouat was born in Finistère on August 7th, 1981. First directed to preparatory classes for Business Schools, she realized that she wanted to make her passion a job : drawing and painting. She joined the Atelier de Sèvres in Paris to prepare her for art school competitive examinations.',20);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Garfield','Richard','Richard Channing Garfield (born June 26, 1963 in Philadelphia, Pennsylvania) is a game designer and mathematics professor who is known for creating the very popular card game Magic: The Gathering.',21);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Bergsala Enigma','Uitgeverij',NULL,NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Conway','Harry','Fantasy illustrator based in Ireland',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Colovini','Leo','Leo Colovini (born 1964 in Venice) is an Italian game designer.',22);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Gifford','Peter','Peter Gifford is a graphic designer from Australia, now living in New Zealand, who is probably best known as the graphic designer for the popular game Tales of the Arabian Nights by Z-Man Games.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('MB Spellen','Uitgeverij',NULL,NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Asmodee','Uitgeverij',NULL,24);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Vuarchex','Thomas','Thomas Vuarchex is a games designer',25);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Vohwinkel','Franz','Franz Vohwinkel (born 1964 in Munich, Germany) is a very prolific game artist. In 1991, he illustrated his first game "Drunter & Drüber" by Klaus Teuber. In 1996, he got the chance to do artwork for Magic: the Gathering.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Ravensburger','Uitgeverij',NULL,26);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Koltze','Rüdiger','Rüdiger Koltze is een Duits spelontwerper',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Bittler','Bernard','Bernard Bittler is een Franse illustrator',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Phalanx Games','Uitgeverij',NULL,27);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Z-Man Games','Uitgeverij',NULL,28);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Leacock','Matt','Matt Leacock is a game designer and user experience designer who is probably best known for creating the very popular game Pandemic. He has been designing games full time since 2014. Prior to that he was a user experience designer at Sococo, Yahoo!, AOL, Netscape, and Apple.',29);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Alea','Uitgeverij',NULL,NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Seyfarth','Andreas','Andreas Seyfarth (born November 6, 1962) is a German-style board game designer who is probably most famous for creating the popular boardgame Puerto Rico. In 1988, he married fellow game designer Karen Seyfarth who helps playtest and design most of his games.',30);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Deep Print Games','Uitgeverij',NULL,31);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Heller','Annika','Annika Heller is an illustrator living in Hamburg, Germany. Born in the Rhoen region, she crafted her first game at the tender age of 10 and went on to eventually study design and illustration. Whenever she is not drawing, you will likely find her either at the table playing games or outside enjoying nature.',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Days of Wonder','Uitgeverij',NULL,32);	
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Moon','Alan R.','Alan R. Moon (born 18 November 1951) is an author of board games, born in Southampton, England. He is generally considered to be one of the foremost designers of German-style board games. Many of his games can be seen as board game variations on the travelling salesman problem.',33);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Daujean','Cyrille','Cyrille Daujean is art director at Days of Wonder',34);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Aporta Games','Uitgeverij',NULL,35);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Svensson','Eilif','Eilif Svensson is originally from Oslo, Norway, but lives now in Mjøndalen, Norway. He is educated as MBE (Master of Business & Economics) and has worked as Finance Manager and Business Controller for several international companies. He started with game designing in 2006 and published a few games for the Norwegian market in the "early years".',36);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Bohne','Gjermund','Norwegian illustrator',NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('999 Games','Uitgeverij',NULL,NULL);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Hargrave','Elizabeth','Elizabeth is a game designer living in Silver Spring, Maryland.',37);
INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Martinez Jaramillo','Ana Maria','Ana María Martínez Jaramillo is a Columbian artist and teacher whose gift for portraying the natural world led her to co-illustrate the popular bird-centric game, “Wingspan,” and a beautiful new companion book.',NULL);

INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('999 Games','999 Games is a Dutch board game publishing company. In Belgium and the Netherlands it publishes mostly German-style board games such as The Settlers of Catan, El Grande and Carcassonne. The company also imports foreign games, such as Magic: the Gathering. In recent years, 999 Games has moved away from gamers games and turned more towards family, kids, or even party games – with Agricola being a popular exception.','https://www.999games.nl/',53,'Nederland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Alea','Imprint of Ravensburger. Publishers of Alea very small box series, Alea small box series (discontinued), Alea medium box series, Alea big box series, and Alea Revised Big Box Series.','http://www.aleaspiele.de/',43,'Duitsland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('AMIGO','Amigo Spiele is a German board and card game publisher. Many of their games have won Spiel des Jahres awards, and many have been published in English by Rio Grande Games.','https://www.amigo-spiele.de/',15,'Duitsland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Aporta Games','Aporta Games is a Norwegian game publisher focusing on Eurostyle family games.','http://www.aportagames.com/',50,'Noorwegen');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Asmodee','Asmodee is a French publisher of board games, card games and role-playing games (RPGs). Founded in 1995 to develop their own games and to publish and distribute for other smaller game developers, they have since acquired numerous other board game publishers.','https://asmodee.com/',34,'Frankrijk');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Bergsala Enigma','This company has been bought by Asmodee, and is now called Asmodee Nordics','http://www.enigmadistribution.com',29,'Denemarken');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Deep Print Games','Deep Print Games is a board game publisher in Berlin owned in equal shares by Peter Eggert and Philipp El Alaoui (formerly with eggertspiele/Plan B Games), Viktor Kobilke (Frosted Games, formerly with eggertspiele/Plan B Games), Matthias Nagy (Frosted Games), and Karsten Esser and Andreas Finkernagel (Pegasus Spiele). Notes the publisher: "The six shareholders bring a total of 130 years of experience in the board game business to the newly-founded publisher, which aims to become a flagship for family games as well as core games and gamers games."','http://www.deep-print-games.com/',45,'Duitsland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Days of Wonder','Days of Wonder publishes top-quality, family-oriented board games that are easy to learn and fun to play. Our name is derived from the "sense of wonder" we all experienced as children when we first fell in love with playing games. Founded in 2002, Days of Wonder has a global presence, with offices in the US and Europe, and distribution in 25 countries. In 2004, Days of Wonder became the youngest publisher ever to win Germanys prestigious Spiel des Jahres, the worlds most coveted game prize, with the launch of its best-selling Ticket to Ride board game series.','https://www.daysofwonder.com/',47,'USA');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Hans im Glück','Hans Im Glück werd opgericht in 1983 door Bernd Brunnhofer en Karl-Heinz Schmiel. De eerste spellen werden met de hand gemaakt in een kleine werkplaats. In 1987 verliet Karl-Heinz Schmiel het bedrijf. Sindsdien is Bernd Brunnhofer de eindverantwoordelijke van de onderneming. In de jaren 80 werden meerdere spellen van Hans Im Glück genomineerd voor de toonaangevende Spiel des Jahres-verkiezing. In 1991 viel die eer te beurt aan het door Hans Im Glück uitgegeven spel Drunter & Drüber van Klaus Teuber. Het was de eerste keer in de geschiedenis dat een klein spellenbedrijf deze wereldwijd prestigieuze prijs won. Heden geldt Hans Im Glück in Duitsland als toonaangevend spellenuitgever. Het grootste succes is het bordspel Carcassonne.','https://www.hans-im-glueck.de/',16,'Duitsland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Libellud','Libellud is sinds 2008 uitgever van bordspellen. Het eerste bordspel dat Libellud publiceerde was het spel Dixit, wat meteen een groot succes werd.','https://www.libellud.com/',25,'Frankrijk');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('MB Spellen','Dutch (Netherlands) branch of Milton Bradley.','https://www.hasbro.com/',33,'Nederland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Next Move Games','Next Move Games is een uitgever die deel uitmaakt van het merk Plan B. Next Move Games concentreert zich op abstracte gezelschapsspellen. Ieder spel heeft een eenvoudig spelmechanisme, maar kan met veel diepgang gespeeld worden.','https://www.nextmovegames.com/',11,'Canada');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('IELLO','With IELLO, a premier games publisher and worldwide distributor in a fast-expanding industry, find all the games you want! IELLO is a reference for board games, both nationally in France, where we come from, and in the world where we are expanding.','https://www.iello.fr/',22,'Frankrijk');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Phalanx Games','Phalanx Games is een spellenuitgeverij die een onderdeel is van 999 Games die zich richt op spellen voor experts.','http://www.phalanxgames.net/',40,'Nederland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Queen Games','Queen Games is een Duitse uitgever van tafelspellen, gevestigd in Troisdorf en in 1992 opgericht door hoofd Rajive Gupta, die zich voornamelijk specialiseert in Duitse stijl, games op familieniveau, maar ook kleinere aantallen heeft gepubliceerd van zowel eenvoudigere, kinderspellen als complexere, games voor gamers.','http://www.queen-games.com/',7,'Duitsland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Ravensburger','Though people tend to think of us as a puzzle company, we like to think of ourselves as so much more. In addition to being a leading supplier of puzzles, games and other products around the world as well as children’s books in Germany, Ravensburger is also a leading provider of jobs in Europe. The blue triangle is one of the most famous trademarks in Germany.','https://www.ravensburger.com/',37,'Duitsland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Repos Production','Repos Production is a European board game publisher, Belgian to be precise, having started its activity by importing Times Up!. This success has allowed them to develop a range of original games of which 7 Wonders is the illustrious representative.','https://www.rprod.com/',8,'België');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Ultra PRO','Ultra PRO International LLC ("Ultra PRO") is the leading manufacturer and supplier of sports and gaming collectibles accessories, photo and scrapbooking albums and pages and Ultra Sleeves™, our line of disposable, protective sleeves for computer tablets.  We have been designing and manufacturing top quality products since 1952, and our brands are recognized for its high quality standards and design innovations.','https://www.ultrapro.com/',19,'USA');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('White Goblin Games','White Goblin Games is a young and fast-growing publisher of board, card and dice games. The company has a broad portfolio of licenses for fun board and card games from numerous international partners. Over the past years White Goblin Games has established themselves nationally as a renowned publisher through the localization of games such as Qwixx, The Mind, Codenames, Machi Koro and many more. Additionally, they develop and publish their ‘own’ titles.','https://whitegoblingames.com/',10,'Nederland');
INSERT INTO [ProjectBGG].[Uitgeverij]
	VALUES ('Z-Man Games','At Z-Man Games, we create innovative games that become modern essentials. Since 1999, our studio has published a variety of critically acclaimed titles from casual to complex, cooperative to competitive, small bag to big box—and everything in between. We combine our keen eye for design, attention to aesthetics, and top-notch components to create the best games for every gamer.','https://www.zmangames.com/',41,'USA');

INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (173346,17,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (20080,15,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (236457,19,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (230802,12,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (11,3,'Duits');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (822,9,'Duits');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (245476,18,'Engels');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (15512,13,'Engels');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (39856,10,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (929,6,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (466,11,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (8098,5,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (1117,16,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (9440,14,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (30549,20,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (3076,2,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (339484,7,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (202670,8,'Nederlands');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (283864,4,'Engels');
INSERT INTO [ProjectBGG].[BordspelUitgeverij]
	VALUES (266192,1,'Nederlands');

INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (173346,1,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (173346,2,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (20080,3,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (20080,4,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (236457,5,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (236457,6,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (230802,11,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (230802,12,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (11,13,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (11,14,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (822,17,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (822,18,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (245476,20,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (245476,21,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (15512,23,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (15512,24,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (39856,26,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (39856,27,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (929,28,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (929,30,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (466,31,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (466,32,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (8098,35,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (8098,36,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (1117,38,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (1117,39,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (9440,11,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (9440,36,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (30549,42,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (30549,12,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (3076,44,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (3076,36,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (339484,11,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (339484,46,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (202670,48,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (202670,49,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (283864,51,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (283864,52,2);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (266192,54,1);
INSERT INTO [ProjectBGG].[BordspelPersoon]
	VALUES (266192,55,2);

INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (173346,1,9);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (20080,1,NULL);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (236457,1,9);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (230802,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (11,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (822,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (245476,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (15512,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (39856,1,7);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (929,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (466,1,NULL);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (8098,1,NULL);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (1117,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (9440,1,NULL);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (30549,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (3076,1,9);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (339484,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (202670,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (283864,1,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (266192,1,9);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (230802,11,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (9440,11,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (339484,11,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (230802,12,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (30549,12,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (8098,36,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (3076,36,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (9440,36,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (173346,2,9);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (230802,2,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (822,2,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (30549,2,NULL);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (3076,2,7);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (339484,2,NULL);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (283864,2,8);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (266192,2,NULL);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (15512,2,7);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (245476,2,5);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (202670,33,10);
INSERT INTO [ProjectBGG].[BordspelGebruiker]
	VALUES (39856,33,5);
