CREATE DATABASE SynergyDB;

CREATE TABLE Races
(
    Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    Origin VARCHAR(255) NOT NULL
);

CREATE TABLE Classes
(
    Id int NOT NULL PRIMARY KEY AUTO_INCREMENT COMMENT 'Primary Key',
    Name VARCHAR(255) NOT NULL,
    Role ENUM("Tank", "Damage", "Healer") NOT NULL,
    Resource ENUM("Fervor", "Focus", "Mana", "Faith", "Energy", "Soul") NOT NULL
);

CREATE TABLE Adventurers
(
    Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    `Rank` ENUM("Rookie", "Veteran", "Elite", "Master", "Legendary") NOT NULL,
    Race int NOT NULL,
    Class int NOT NULL,
    Foreign Key (Race) REFERENCES Races(Id),
    Foreign Key (Class) REFERENCES Classes(Id)
);

CREATE TABLE Parties
(
    Id int NOT NULL PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    DateFounded DATETIME
);

ALTER TABLE Adventurers ADD Party int NOT NULL;

ALTER TABLE Adventurers
ADD CONSTRAINT Party Foreign Key (Party) REFERENCES Parties(Id);
