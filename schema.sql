CREATE DATABASE fournisseur_identite;
\c fournisseur_identite
CREATE TABLE users(
    id serial PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    mail VARCHAR(50) NOT NULL UNIQUE,
    mdp VARCHAR(32) NOT NULL,
    date_inscription TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);