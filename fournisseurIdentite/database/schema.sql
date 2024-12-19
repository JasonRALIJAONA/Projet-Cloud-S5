CREATE DATABASE fournisseur_identite;
\c fournisseur_ide ntite
CREATE TABLE utilisateur(
    id serial PRIMARY KEY, 
    nom_utilisateur varchar(50) unique,
    email varchar(50) unique,
    mot_de_passe varchar(50) not null,
    est_valide boolean default false,
    nb_tentative integer default 0
);
