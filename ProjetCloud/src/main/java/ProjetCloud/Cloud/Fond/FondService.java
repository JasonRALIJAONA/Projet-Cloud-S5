package ProjetCloud.Cloud.Fond;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class FondService {
    @Autowired
    private FondRepository FondRepository;

    public List<Fond> getAllFonds() {
        return FondRepository.findAll();
    }

    public Fond getFond(Long id) {
        return FondRepository.findById(id).orElse(null);
    }

    public Fond createFond(Fond Fond) {
        return FondRepository.save(Fond);
    }

    public Fond updateFond(Long id, Fond Fond) {
        if (FondRepository.existsById(id)) {
            Fond.setId(id);
            return FondRepository.save(Fond);
        }
        return null;
    }

    public void deleteFond(Long id) {
        FondRepository.deleteById(id);
    }
}




