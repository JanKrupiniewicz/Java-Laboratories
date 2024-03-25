package org.example.entity;

import jakarta.persistence.*;
import lombok.*;
import org.example.entity.Mage;

import java.util.ArrayList;
import java.util.List;

@Entity @NoArgsConstructor @Table (name="Towers")
public class Tower {
    @Id @Getter
    private String name;

    @Getter @Setter
    private int height;

    @Getter @OneToMany(mappedBy = "tower")
    private List<Mage> mages;
    public Tower(String name, int height) {
        this.name = name;
        this.height = height;
        mages = new ArrayList<>();
    }
    public void addMage(Mage mage) {
        this.mages.add(mage);
    }
    public void deleteMage(Mage mage) {
        this.mages.remove(mage);
    }
}