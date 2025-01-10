package ProjetCloud.Cloud.Transaction;

import javax.persistence.*;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
@Table(name = "transaction")
public class Transaction {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    @JoinColumn(name = "utilisateur_id", referencedColumnName = "id")
    private Utilisateur utilisateur;

    @ManyToOne
    @JoinColumn(name = "cryptomonnaie_id", referencedColumnName = "id")
    private Cryptomonnaie cryptomonnaie;

    @Column(name = "achat", length = 10)
    private String achat;

    @Column(name = "vente", length = 10)
    private String vente;

    @Column(name = "quantite", precision = 15, scale = 8)
    private BigDecimal quantite;

    @Column(name = "montant", precision = 15, scale = 8)
    private BigDecimal montant;

    @Column(name = "valide")
    private Boolean valide;

    @Column(name = "date_transaction", columnDefinition = "TIMESTAMP DEFAULT CURRENT_TIMESTAMP")
    private LocalDateTime dateTransaction;

    public Transaction() {
    }

    public Transaction(Long id, Utilisateur utilisateur, Cryptomonnaie cryptomonnaie, String achat, String vente,
            BigDecimal quantite, BigDecimal montant, Boolean valide, LocalDateTime dateTransaction) {
        this.id = id;
        this.utilisateur = utilisateur;
        this.cryptomonnaie = cryptomonnaie;
        this.achat = achat;
        this.vente = vente;
        this.quantite = quantite;
        this.montant = montant;
        this.valide = valide;
        this.dateTransaction = dateTransaction;
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

    public String getAchat() {
        return achat;
    }

    public void setAchat(String achat) {
        this.achat = achat;
    }

    public String getVente() {
        return vente;
    }

    public void setVente(String vente) {
        this.vente = vente;
    }

    public BigDecimal getQuantite() {
        return quantite;
    }

    public void setQuantite(BigDecimal quantite) {
        this.quantite = quantite;
    }

    public BigDecimal getMontant() {
        return montant;
    }

    public void setMontant(BigDecimal montant) {
        this.montant = montant;
    }

    public Boolean getValide() {
        return valide;
    }

    public void setValide(Boolean valide) {
        this.valide = valide;
    }

    public LocalDateTime getDateTransaction() {
        return dateTransaction;
    }

    public void setDateTransaction(LocalDateTime dateTransaction) {
        this.dateTransaction = dateTransaction;
    }

}
