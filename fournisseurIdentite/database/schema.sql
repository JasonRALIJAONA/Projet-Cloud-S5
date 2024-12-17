CREATE DATABASE fournisseur_identite;
\c fournisseur_identite
CREATE TABLE Users(
    id serial PRIMARY KEY,
    username varchar(50) unique,
    email varchar(50) unique,
    pass varchar(50) not null
);