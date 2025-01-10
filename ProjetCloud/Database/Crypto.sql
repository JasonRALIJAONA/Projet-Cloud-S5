CREATE DATABASE Crypto;
\c Crypto;

CREATE TABLE utilisateur (
    id SERIAL PRIMARY KEY,
    nom_utilisateur VARCHAR(50) UNIQUE,
    email VARCHAR(50) UNIQUE,
    mot_de_passe VARCHAR(255) NOT NULL,
    token VARCHAR(255) NOT NULL,
    date_creation_compte TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE cryptomonnaie (
    id SERIAL PRIMARY KEY,
    nom VARCHAR(50) UNIQUE,
    symbole VARCHAR(10) UNIQUE,
    prix DECIMAL(15, 8) DEFAULT 0,
    variation DECIMAL(5, 2) DEFAULT 0
);

CREATE TABLE portefeuille (
    id SERIAL PRIMARY KEY,
    utilisateur_id INTEGER REFERENCES utilisateur(id) ON DELETE CASCADE,
    cryptomonnaie_id INTEGER REFERENCES cryptomonnaie(id),
    quantite DECIMAL(15, 8) DEFAULT 0
);

CREATE TABLE transaction (
    id SERIAL PRIMARY KEY,
    utilisateur_id INTEGER REFERENCES utilisateur(id),
    cryptomonnaie_id INTEGER REFERENCES cryptomonnaie(id),
    achat VARCHAR(10),
    vente VARCHAR(10),
    quantite DECIMAL(15, 8),
    montant DECIMAL(15, 8),
    valide BOOLEAN,
    date_transaction TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE fond (
    id SERIAL PRIMARY KEY,
    utilisateur_id INTEGER REFERENCES utilisateur(id),
    depot VARCHAR(10),
    retrait VARCHAR(10),
    montant DECIMAL(15, 2),
    date_fond TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
