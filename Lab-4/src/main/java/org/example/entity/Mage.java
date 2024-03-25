package org.example.entity;

import jakarta.persistence.*;
import lombok.*;

@Entity @NoArgsConstructor @Table(name="Mages")
public class Mage {
    @Getter @Id
    private String name;

    @Getter @Setter
    private int level;

    @ManyToOne @Getter @Setter
    private Tower tower;
    public Mage(String name, int level, Tower tower) {
        this.name = name;
        this.level = level;
        this.tower = tower;
        tower.addMage(this);
    }
    public void deleteTower() {
        this.tower = null;
    }
}
