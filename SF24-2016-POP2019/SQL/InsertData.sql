INSERT INTO Ustanova (SifraUstanove,Naziv,Lokacija,Obrisano) VALUES ('1', 'Ustanova 1', 'Novi Sad', 0);
INSERT INTO Ustanova (SifraUstanove,Naziv,Lokacija,Obrisano) VALUES ('2', 'Ustanova 2', 'Beograd', 0);
INSERT INTO Ustanova (SifraUstanove,Naziv,Lokacija,Obrisano) VALUES ('3', 'Ustanova 3', 'Sid', 0);

INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Darko', 'Greksa', 'greksadarko@gmail.com', 'darko','123', 'Administrator', 0, null);
INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Petar', 'Petrovic', 'pera@gmail.com', 'pera', '123', 'Profesor', 0, 1);
INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Mirko', 'Mirkovic', 'mirko@gmail.com', 'mirko', '123', 'Profesor', 0, 2);
INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Laza', 'Lazic', 'laza@gmail.com', 'laza', '123', 'Asistent', 0, 1);
INSERT INTO Korisnik (Ime,Prezime,Email,Username,Password,TipKorisnika,Obrisano,UstanovaId) VALUES ('Jovana', 'Jovanovic', 'jj@gmail.com', 'jovana', '123', 'Asistent', 0, 2);


INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano) VALUES (1, 20, 'SaRacunarima', 0);
INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano) VALUES (2, 80, 'BezRacunara', 0);
INSERT INTO Ucionica (BrojUcionice,BrojMesta,TipUcionice,Obrisano) VALUES (3, 30, 'SaRacunarima', 0);

INSERT INTO Termin(TipNastave,VremeZauzecaOd,VremeZauzecaDo,Dan,KorisnikId,Obrisano) VALUES ('Predavanje','2019-12-12 13:00', '2019-12-12 14:30', 'Cetvrtak', 2, 0);
INSERT INTO Termin(TipNastave,VremeZauzecaOd,VremeZauzecaDo,Dan,KorisnikId,Obrisano) VALUES ('Vezbe', '2019-12-13 15:00', '2019-12-13 17:00', 'Petak', 2, 0);
INSERT INTO Termin(TipNastave,VremeZauzecaOd,VremeZauzecaDo,Dan,KorisnikId,Obrisano) VALUES ('Predavanje', '2019-12-13 18:00', '2019-12-13 21:00', 'Petak', 3, 0);
