package ProjetCloud.Cloud.Fond;

import javax.persistence.*;
import java.math.BigDecimal;
import java.time.LocalDateTime;

import ProjetCloud.Cloud.Utilisateur.*;

@Entity
@Table(name = "fond")
public class Fond {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "utilisateur_id", referencedColumnName = "id")
    private Utilisateur utilisateur;

    @Column(name = "depot", length = 10)
    private String depot;

    @Column(name = "retrait", length = 10)
    private String retrait;

    @Column(name = "montant", precision = 15, scale = 2)
    private BigDecimal montant;

    @Column(name = "date_fond", columnDefinition = "TIMESTAMP DEFAULT CURRENT_TIMESTAMP")
    private LocalDateTime dateFond;

    // Getters and setters
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

    public String getDepot() {
        return depot;
    }

    public void setDepot(String depot) {
        this.depot = depot;
    }

    public String getRetrait() {
        return retrait;
    }

    public void setRetrait(String retrait) {
        this.retrait = retrait;
    }

    public BigDecimal getMontant() {
        return montant;
    }

    public void setMontant(BigDecimal montant) {
        this.montant = montant;
    }

    public LocalDateTime getDateFond() {
        return dateFond;
    }

    public void setDateFond(LocalDateTime dateFond) {
        this.dateFond = dateFond;
    }

    // Constructors
    public Fond() {
    }

    public Fond(Utilisateur utilisateur, String depot, String retrait, BigDecimal montant) {
        this.utilisateur = utilisateur;
        this.depot = depot;
        this.retrait = retrait;
        this.montant = montant;
        this.dateFond = LocalDateTime.now();
    }

    @Override
    public String toString() {
        return "Fond{" +
                "id=" + id +
                ", utilisateur=" + utilisateur +
                ", depot='" + depot + '\'' +
                ", retrait='" + retrait + '\'' +
                ", montant=" + montant +
                ", dateFond=" + dateFond +
                '}';
    }
}
