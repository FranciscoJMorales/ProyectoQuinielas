CREATE SCHEMA `quinielas` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE quinielas;

CREATE TABLE Users(
  id INTEGER PRIMARY KEY AUTO_INCREMENT,
  username VARCHAR(255) NOT NULL,
  email VARCHAR(255) NOT NULL,
  password VARCHAR(255) NOT NULL,
  active BIT NOT NULL DEFAULT 1
);

CREATE TABLE Pools(
  id INTEGER PRIMARY KEY AUTO_INCREMENT,
  admin_id INTEGER NOT NULL,
  name VARCHAR(255) NOT NULL,
  private BIT NOT NULL,
  password VARCHAR(255) NULL,
  users_limit INTEGER NOT NULL,
  active BIT NOT NULL DEFAULT 1,
  FOREIGN KEY (admin_id) REFERENCES Users(id)
);

CREATE TABLE UsersPools(
  user_id INTEGER NOT NULL,
  pool_id INTEGER NOT NULL,
  PRIMARY KEY (user_id, pool_id),
  FOREIGN KEY (user_id) REFERENCES Users(id),
  FOREIGN KEY (pool_id) REFERENCES Pools(id)
);

CREATE TABLE Games(
  id INTEGER PRIMARY KEY AUTO_INCREMENT,
  pool_id INTEGER NOT NULL,
  team1 VARCHAR(255) NOT NULL,
  team2 VARCHAR(255) NOT NULL,
  team1_score INTEGER NULL,
  team2_score INTEGER NULL,
  game_date DATETIME NOT NULL,
  deadline DATETIME NOT NULL,
  active BIT NOT NULL DEFAULT 1,
  FOREIGN KEY (pool_id) REFERENCES Pools(id)
);

CREATE TABLE Predictions(
  game_id INTEGER NOT NULL,
  user_id INTEGER NOT NULL,
  team1_score INTEGER NULL,
  team2_score INTEGER NULL,
  score INTEGER NULL,
  active BIT NOT NULL DEFAULT 1,
  PRIMARY KEY (game_id, user_id),
  FOREIGN KEY (game_id) REFERENCES Games(id),
  FOREIGN KEY (user_id) REFERENCES Users(id)
);
