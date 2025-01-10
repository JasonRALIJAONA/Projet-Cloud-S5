package ProjetCloud.Cloud.Portefeuille;

import jakarta.persistence.*;
import java.math.BigDecimal;

import ProjetCloud.Cloud.Utilisateur.*;
import ProjetCloud.Cloud.Cryptomonnaie.*;

@Entity
public class Portefeuille {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "utilisateur_id", nullable = false)
    private Utilisateur utilisateur;

    @ManyToOne
    @JoinColumn(name = "cryptomonnaie_id", nullable = false)
    private Cryptomonnaie cryptomonnaie;

    @Column(precision = 15, scale = 8)
    private BigDecimal quantite = BigDecimal.ZERO;

    public Portefeuille() {
    }

    public Portefeuille(Long id, Utilisateur utilisateur, Cryptomonnaie cryptomonnaie, BigDecimal quantite) {
        this.id = id;
        this.utilisateur = utilisateur;
        this.cryptomonnaie = cryptomonnaie;
        this.quantite = quantite;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Utilisateur getUtilisateur() {
        return utilisateur;
    }

    public void setUtilisateur(Utilisateur utilisateur) {
        this.utilisateur = utilisateur;
    }

    public Cryptomonnaie getCryptomonnaie() {
        return cryptomonnaie;
    }

    public void setCryptomonnaie(Cryptomonnaie cryptomonnaie) {
        this.cryptomonnaie = cryptomonnaie;
    }

    public BigDecimal getQuantite() {
        return quantite;
    }

    public void setQuantite(BigDecimal quantite) {
        this.quantite = quantite;
    }

}
