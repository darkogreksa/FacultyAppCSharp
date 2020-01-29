

CREATE TABLE Ustanova (
Id INT PRIMARY KEY IDENTITY(1, 1),
SifraUstanove VARCHAR(3),
Naziv VARCHAR(20),
Lokacija VARCHAR(20),
Obrisano BIT
)

INSERT INTO Ustanova (SifraUstanove,Naziv,Lokacija,Obrisano) VALUES ('1', 'FTN', 'Novi Sad', 0);
INSERT INTO Ustanova (SifraUstanove,Naziv,Lokacija,Obrisano) VALUES ('2', 'FON', 'Beograd', 0);
INSERT INTO Ustanova (SifraUstanove,Naziv,Lokacija,Obrisano) VALUES ('3', 'PMF', 'Novi Sad', 0);

CREATE TABLE Korisnik (
Id INT PRIMARY KEY IDENTITY(1, 1),
Ime VARCHAR(10),
Prezime VARCHAR(20),
Email VARCHAR(30),
Username VARCHAR(20),
Password VARCHAR(20),
TipKorisnika VARCHAR(20) CHECK(TipKorisnika IN ('Administrator','Profesor','Asistent')),
Obrisano BIT,
UstanovaId INT,
FOREIGN KEY (UstanovaId) REFERENCES Ustanova(Id)
)

INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Darko', 'Greksa', 'greksadarko@gmail.com', 'darko','123', 'Administrator', 0, 1);
INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Petar', 'Petrovic', 'pera@gmail.com', 'pera', '123', 'Profesor', 0, 2);
INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Mirko', 'Mirkovic', 'mirko@gmail.com', 'mirko', '123', 'Profesor', 0, 3);
INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Laza', 'Lazic', 'laza@gmail.com', 'laza', '123', 'Asistent', 0, 2);
INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Jovana', 'Jovanovic', 'jj@gmail.com', 'jovana', '123', 'Asistent', 0, 3);


CREATE TABLE Ucionica (
Id INT PRIMARY KEY IDENTITY(1, 1),
BrojUcionice INT,
BrojMesta INT,
TipUcionice VARCHAR(20) CHECK(TipUcionice IN ('SaRacunarima','BezRacunara')),
Obrisano BIT,
UstanovaId INT
)

INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano,UstanovaId) VALUES (301, 20, 'SaRacunarima', 0, 1);
INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano,UstanovaId) VALUES (442, 80, 'BezRacunara', 0, 2);
INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano,UstanovaId) VALUES (103, 30, 'SaRacunarima', 0, 3);
INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano,UstanovaId) VALUES (302, 30, 'SaRacunarima', 0, 1);
INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano,UstanovaId) VALUES (443, 30, 'BezRacunara', 0, 2);
INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano,UstanovaId) VALUES (102, 30, 'SaRacunarima', 0, 3);


CREATE TABLE Termin (
Id INT PRIMARY KEY IDENTITY(1, 1),
TipNastave VARCHAR(20) CHECK(TipNastave IN ('Predavanje','Vezbe')),
VremeZauzecaOd DATETIME,
VremeZauzecaDo DATETIME,
Dan VARCHAR(10),
KorisnikId INT,
Obrisano BIT,
FOREIGN KEY (KorisnikId) REFERENCES Korisnik(Id)
)

INSERT INTO Termin(TipNastave,VremeZauzecaOd,VremeZauzecaDo,Dan,KorisnikId,Obrisano) VALUES ('Predavanje','2019-12-12 13:00', '2019-12-12 14:30', 'Cetvrtak', 2, 0);
INSERT INTO Termin(TipNastave,VremeZauzecaOd,VremeZauzecaDo,Dan,KorisnikId,Obrisano) VALUES ('Vezbe', '2019-12-13 15:00', '2019-12-13 17:00', 'Petak', 2, 0);
INSERT INTO Termin(TipNastave,VremeZauzecaOd,VremeZauzecaDo,Dan,KorisnikId,Obrisano) VALUES ('Predavanje', '2019-12-13 18:00', '2019-12-13 21:00', 'Petak', 3, 0);
