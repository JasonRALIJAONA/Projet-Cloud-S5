CREATE DATABASE fournisseur_identite;
\c fournisseur_identite
CREATE TABLE users(
    id serial PRIMARY KEY,
    username varchar(50) unique,
    email varchar(50) unique,
    pass varchar(50) not null,
    est_valide boolean default false,
    nb_tentative integer default 0
);

