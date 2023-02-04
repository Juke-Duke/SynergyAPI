# SynergyAPI

<details>
### Table of Contents
- [SynergyAPI](#synergyapi)
    - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Getting Started](#getting-started)
    - [Setting Up The Database](#setting-up-the-database)
    - [Seeding Races And Classes](#seeding-races-and-classes)
    - [Enums](#enums)
  - [API Reference](#api-reference)
    - [Adventurer](#adventurer)
      - [Adventurer Model](#adventurer-model)
      - [Get All Adventurers Endpoint](#get-all-adventurers-endpoint)
      - [Get All Adventurers Response](#get-all-adventurers-response)
      - [Get Adventurer By Id Endpoint](#get-adventurer-by-id-endpoint)
      - [Get Adventurer By Id Response](#get-adventurer-by-id-response)
      - [Create Adventurer Endpoint](#create-adventurer-endpoint)
      - [Create Adventurer Request](#create-adventurer-request)
      - [Create Adventurer Response](#create-adventurer-response)
      - [Update Adventurer By Id Endpoint](#update-adventurer-by-id-endpoint)
      - [Update Adventurer By Id Request](#update-adventurer-by-id-request)
      - [Update Adventurer By Id Response](#update-adventurer-by-id-response)
      - [Delete Adventurer By Id Endpoint](#delete-adventurer-by-id-endpoint)
      - [Delete Adventurer By Id Response](#delete-adventurer-by-id-response)
    - [Party](#party)
      - [Party Model](#party-model)
      - [Get All Parties Endpoint](#get-all-parties-endpoint)
      - [Get All Parties Response](#get-all-parties-response)
      - [Get Party By Id Endpoint](#get-party-by-id-endpoint)
      - [Get By Party By Id Response](#get-by-party-by-id-response)
      - [Create Party Endpoint](#create-party-endpoint)
      - [Create Party Request](#create-party-request)
      - [Create Party Response](#create-party-response)
      - [Update Party By Id Endpoint](#update-party-by-id-endpoint)
      - [Update Party By Id Request](#update-party-by-id-request)
      - [Update Party By Id Response](#update-party-by-id-response)
      - [Delete Party By Id Endpoint](#delete-party-by-id-endpoint)
      - [Delete Party By Id Response](#delete-party-by-id-response)
    - [Race](#race)
      - [Race Model](#race-model)
      - [Get All Races Endpoint](#get-all-races-endpoint)
      - [Get All Races Response](#get-all-races-response)
      - [Get Race By Name Endpoint](#get-race-by-name-endpoint)
      - [Get Race By Name Response](#get-race-by-name-response)
    - [Class](#class)
      - [Class Model](#class-model)
      - [Get All Classes Endpoint](#get-all-classes-endpoint)
      - [Get All Classes Response](#get-all-classes-response)
      - [Get Class By Name Endpoint](#get-class-by-name-endpoint)
      - [Get Class By Name Response](#get-class-by-name-response)
</details>

## Introduction
I’ve always loved the fantasy genre and this backend service representation really encapsulates a lot of my favorite aspects of the common role-playing tropes. One of the best things about rpgs is the interactions and experiences one shares with others, so making a service to connect adventurers would only add on to this positive effect.

![RPGParty](https://mir-s3-cdn-cf.behance.net/project_modules/fs/e47aac98992561.5f3734054aa5d.jpg)

## Getting Started

### Setting Up The Database
This project uses a MySQL database. To set up the database, run the following commands in the MySQL shell, the sql file is also provided in the repository:

```sql
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
```

### Seeding Races And Classes
Once the database is all setup, you can now begin seeding the Races and Classes of this fantasy world. To do so, simply run the API and the seeding process should beging. This is ony done the first time you run the API.

```cs
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SynergyDbContext>();
    RaceSeed.Seed(db);
    ClassSeed.Seed(db);
}
```

The following races available:

```
Human,
Lush Elf,
Dwarf,
Mist Elf,
Giant,
Orc
```

The following classes available:

```
Warrior,
Marksman,
Mage,
Cleric,
Paladin,
Rogue,
Reaper
```
### Enums
These are enums that may be used in some requests:

```cs
public enum Rank
{
    Rookie,
    Veteran,
    Elite,
    Master,
    Legendary
}

public enum Role
{
    Tank,
    Damage,
    Healer
}

public enum Resource
{
    Fervor,
    Focus,
    Mana,
    Faith,
    Energy,
    Soul
}
```

## API Reference

### Adventurer

#### Adventurer Model
| Property | Type | Description |
| --- | --- | --- |
| Id | Int | Primary Key |
| Name | String | Adventurer's name |
| Rank | [Rank](#enums) | Current ranking |
| Race | [Race](#enums) | Adventurer's race |
| Class | [Class](#enums) | Adventurer's class |
| Party | [Party?](#party) | Adventurer's party |

#### Get All Adventurers Endpoint
```http
GET https://localhost:7000/api/adventurer
```

#### Get All Adventurers Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - All registered adventurers",
    "data": [
        {
            "id": 1,
            "name": "Rouge Voltgale",
            "rank": "Elite",
            "race": {
                "id": 1,
                "name": "Human",
                "origin": "Thundorum City"
            },
            "class": {
                "id": 6,
                "name": "Rogue",
                "role": "Damage",
                "resource": "Energy"
            },
            "party": {
                "id": 1,
                "name": "Origin Seekers",
                "dateFounded": "2020-10-05T00:00:00"
            }
        },
        {
            "id": 2,
            "name": "Gildun Fullbash",
            "rank": "Elite",
            "race": {
                "id": 1,
                "name": "Human",
                "origin": "Thundorum City"
            },
            "class": {
                "id": 5,
                "name": "Paladin",
                "role": "Tank",
                "resource": "Faith"
            },
            "party": {
                "id": 1,
                "name": "Origin Seekers",
                "dateFounded": "2020-01-01T00:00:00"
            },
            ...
        }
    ]
}
```

#### Get Adventurer By Id Endpoint
```http
GET https://localhost:7000/api/adventurer/{id}
```

#### Get Adventurer By Id Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - Adventurer with id 4 found",
    "data": {
        "id": 4,
        "name": "Celuna Silverstreak",
        "rank": "Elite",
        "race": {
            "id": 2,
            "name": "Lush Elf",
            "origin": "Serinii Valush"
        },
        "class": {
            "id": 2,
            "name": "Marksman",
            "role": "Damage",
            "resource": "Focus"
        },
        "party": {
            "id": 1,
            "name": "Origin Seekers",
            "dateFounded": "2020-01-01T00:00:00"
        }
    }
}
```

#### Create Adventurer Endpoint
```http
POST https://localhost:7000/api/adventurer
```

#### Create Adventurer Request
```json
{
    "name": "Finn the Human",
    "rank": "Legendary",
    "raceName": "Human",
    "className": "Warrior",
    "partyId": 2
}
```

#### Create Adventurer Response
```json
{
    "statusCode": 201,
    "statusDescription": "Created - Adventurer with Id 7 created",
    "data": {
        "id": 7,
        "name": "Finn the Human",
        "rank": "Legendary",
        "race": {
            "id": 1,
            "name": "Human",
            "origin": "Thundorum City"
        },
        "class": {
            "id": 1,
            "name": "Warrior",
            "role": "Tank",
            "resource": "Fervor"
        },
        "party": {
            "id": 2,
            "name": "The Treehouse",
            "dateFounded": "2010-10-05T00:00:00"
        }
    }
}
```

#### Update Adventurer By Id Endpoint
```http
PUT https://localhost:7000/api/adventurer/{id}
```

#### Update Adventurer By Id Request
```json
{
    "name": "Finn the Legendary Human",
    "rank": "Legendary",
    "raceName": "Human",
    "className": "Warrior",
    "partyId": 3
}
```

#### Update Adventurer By Id Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - Adventurer with Id 7 updated",
    "data": {
        "id": 7,
        "name": "Finn the Legendary Human",
        "rank": "Legendary",
        "race": {
            "id": 1,
            "name": "Human",
            "origin": "Thundorum City"
        },
        "class": {
            "id": 1,
            "name": "Warrior",
            "role": "Tank",
            "resource": "Fervor"
        },
        "party": {
            "id": 3,
            "name": "Hall of Champions",
            "dateFounded": "2021-12-20T02:08:30"
        }
    }
}
```

#### Delete Adventurer By Id Endpoint
```http
DELETE https://localhost:7000/api/adventurer/{id}
```

#### Delete Adventurer By Id Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - Adventurer with Id 7 deleted",
    "data": {
                "id": 7,
        "name": "Finn the Legendary Human",
        "rank": "Legendary",
        "race": {
            "id": 1,
            "name": "Human",
            "origin": "Thundorum City"
        },
        "class": {
            "id": 1,
            "name": "Warrior",
            "role": "Tank",
            "resource": "Fervor"
        },
        "party": {
            "id": 3,
            "name": "Hall of Champions",
            "dateFounded": "2021-12-20T02:08:30"
        }
    }
}
```

### Party

#### Party Model
| Property | Type | Description |
| --- | --- | --- |
| Id | Int | Primary Key |
| Name | String | Party's name |
| Date Founded | DateTime | Party's founding date |
| Members | [[Adventurer]](#adventurer) | Party's members |

#### Get All Parties Endpoint
```http
GET https://localhost:7000/api/party
```

#### Get All Parties Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - All registered parties",
    "data": [
        {
            "id": 1,
            "name": "Origin Seekers",
            "dateFounded": "2020-01-01T00:00:00",
            "members": [
                {
                    "id": 1,
                    "name": "Rouge Voltgale",
                    "rank": "Elite",
                    "race": {
                        "id": 1,
                        "name": "Human",
                        "origin": "Thundorum City"
                    },
                    "class": {
                        "id": 3,
                        "name": "Rogue",
                        "role": "Damage",
                        "resource": "Energy"
                    }
                },
                {
                    "id": 2,
                    "name": "Gildun Fullbash",
                    "rank": "Elite",
                    "race": {
                        "id": 1,
                        "name": "Human",
                        "origin": "Thundorum City"
                    },
                    "class": {
                        "id": 5,
                        "name": "Paladin",
                        "role": "Tank",
                        "resource": "Faith"
                    }
                },
                {
                    "id": 4,
                    "name": "Celuna Silverstreak",
                    "rank": "Elite",
                    "race": {
                        "id": 2,
                        "name": "Lush Elf",
                        "origin": "Serinii Valush"
                    },
                    "class": {
                        "id": 2,
                        "name": "Marksman",
                        "role": "Damage",
                        "resource": "Focus"
                    }
                }
            ]
        },
        {
            "id": 3,
            "name": "Hall of Champions",
            "dateFounded": "2021-12-20T02:08:30",
            "members": [
                {
                    "id": 7,
                    "name": "Finn the Legendary Human",
                    "rank": "Legendary",
                    "race": {
                        "id": 1,
                        "name": "Human",
                        "origin": "Thundorum City"
                    },
                    "class": {
                        "id": 1,
                        "name": "Warrior",
                        "role": "Tank",
                        "resource": "Fervor"
                    }
                }
            ]
        },
        ...
    ]
}
```

#### Get Party By Id Endpoint
```http
GET https://localhost:7000/api/party/{id}
```

#### Get By Party By Id Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - Party with Id 3 found",
    "data": {
        "id": 3,
        "name": "Hall of Champions",
        "dateFounded": "2021-12-20T02:08:30",
        "members": [
            {
                "id": 7,
                "name": "Finn the Legendary Human",
                "rank": "Legendary",
                "race": {
                    "id": 1,
                    "name": "Human",
                    "origin": "Thundorum City"
                },
                "class": {
                    "id": 1,
                    "name": "Warrior",
                    "role": "Tank",
                    "resource": "Fervor"
                }
            }
        ]
    }
}
```

#### Create Party Endpoint
```http
POST https://localhost:7000/api/party
```

#### Create Party Request
```json
{
    "name": "Adventurers Guild"
}
```

#### Create Party Response
```json
{
    "statusCode": 201,
    "statusDescription": "Created - Party with Id 4 created",
    "data": {
        "id": 4,
        "name": "Adventurers Guild",
        "dateFounded": "2022-11-25T10:00:00",
        "members": []
    }
}
```

#### Update Party By Id Endpoint
```http
PUT https://localhost:7000/api/party/{id}
```

#### Update Party By Id Request
```json
{
    "name": "Thee Adventurers Guild"
}
```

#### Update Party By Id Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - Party with Id 4 updated",
    "data": {
        "id": 4,
        "name": "Thee Adventurers Guild",
        "dateFounded": "2022-11-25T10:00:00",
        "members": []
    }
}
```

#### Delete Party By Id Endpoint
```http
DELETE https://localhost:7000/api/party/{id}
```

#### Delete Party By Id Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - Party with Id 4 deleted",
    "data": {
        "id": 4,
        "name": "Thee Adventurers Guild",
        "dateFounded": "2022-11-25T10:00:00",
        "members": []
    }
}
```

### Race

#### Race Model
| Property | Type | Description |
| --- | --- | --- |
| Id | Int | Primary Key |
| Name | String | Name of the race |
| Origin | String | Name of the race origin location |

#### Get All Races Endpoint
```http
GET https://localhost:7000/api/race
```

#### Get All Races Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - All available races",
    "data": [
        {
            "id": 1,
            "name": "Human",
            "origin": "Thundorum City"
        },
        {
            "id": 2,
            "name": "Lush Elf",
            "origin": "Serinii Valush"
        },
        {
            "id": 3,
            "name": "Dwarf",
            "origin": "Stoneborn Mountain"
        },
        {
            "id": 4,
            "name": "Mist Elf", 
            "origin": "Miswood Forest"
        },
        {
            "id": 5,
            "name": "Giant",
            "origin": "Jotunheim"
        },
        {
            "id": 6,
            "name": "Orc",
            "origin": "Red Valley"
        }
    ]
}
```

#### Get Race By Name Endpoint
```http
GET https://localhost:7000/api/race/{name}
```

#### Get Race By Name Response
````json
{
    "statusCode": 200,
    "statusDescription": "OK - Race with name Dwarf found.",
    "data": {
        "id": 3,
        "name": "Dwarf",
        "origin": "Stoneborn Mountain"
    }
}
````

### Class

#### Class Model
| Property | Type | Description |
| --- | --- | --- |
| Id | Int | Primary Key |
| Name | String | Name of the class |
| Role | [Role](#enums) | The role the class plays |
| Resource | [Resource](#enums) | The resource used for abilities |

#### Get All Classes Endpoint
```http
GET https://localhost:7000/api/class
```

#### Get All Classes Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - All available classes",
    "data": [
        {
            "id": 1,
            "name": "Warrior",
            "role": "Tank",
            "resource": "Fervor"
        },
        {
            "id": 2,
            "name": "Marksman",
            "role": "Damage",
            "resource": "Focus"
        },
        {
            "id": 3,
            "name": "Mage",
            "role": "Damage",
            "resource": "Mana"
        },
        {
            "id": 4,
            "name": "Cleric",
            "role": "Healer",
            "resource": "Faith"
        },
        {
            "id": 5,
            "name": "Paladin",
            "role": "Tank",
            "resource": "Faith"
        },
        {
            "id": 6,
            "name": "Rogue",
            "role": "Damage",
            "resource": "Energy"
        },
        {
            "id": 7,
            "name": "Reaper",
            "role": "Damage",
            "resource": "Soul"
        }
    ]
}
```

#### Get Class By Name Endpoint
```http
GET https://localhost:7000/api/class/{name}
```

#### Get Class By Name Response
```json
{
    "statusCode": 200,
    "statusDescription": "OK - Class with name Reaper found.",
    "data": {
        "id": 7,
        "name": "Reaper",
        "role": "Damage",
        "resource": "Soul"
    }
}
```

Youssef Elshabasy 2022 - CSCI 39537 Intro to APIs - Hunter College Fall 2022
