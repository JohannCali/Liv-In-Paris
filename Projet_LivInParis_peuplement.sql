INSERT INTO Utilisateur (Id_Utilisateur, Nom, Prenom, Adresse, Email, Mot_de_Passe, Numero_de_Telephone) VALUES
('b21d7c5e-4d6a-4e34-81a2-d7cbbd942311', 'Martin', 'Alice', '12 Rue des Lilas, 75012 Paris', 'alice.martin@mail.com', 'pass1234', '0612345678'),
('a94e23b2-8e44-41a5-92bc-4b1a6b159e02', 'Dupont', 'Jean', '34 Avenue Victor Hugo, 75016 Paris', 'jean.dupont@mail.com', 'azerty', '0698765432'),
('e37a3c9e-58fa-4563-a999-2a5c79a19c09', 'Nguyen', 'Linh', '5 Boulevard Haussmann, 75009 Paris', 'linh.nguyen@mail.com', 'motdepasse', '0601020304'),
('c6188aeb-3f3e-4ff2-9f44-8efb2c8e6dc0', 'Sow', 'Mamadou', '28 Rue de la République, 75020 Paris', 'mamadou.sow@mail.com', 'mdp123', '0678901234');


INSERT INTO Cuisinier (Id_Utilisateur, Specialite) VALUES
('d88fbe71-95a4-42dc-b5c3-ec2a9fd1b01e', 'Cuisine Italienne'),
('f473dce6-3a2c-4c2c-8e15-2f21a5db9f7a', 'Cuisine Libanaise'),
('a09127db-d57f-4d90-8705-342382071346', 'Cuisine Végétarienne'),
('b5ee7929-07e0-4e4e-a6b4-7f29e4bb0d3d', 'Cuisine Africaine'),
('c2f9517a-b569-4534-8e3b-e3aa5db79e62', 'Cuisine Japonaise'),
('bc3d35f1-8ff1-417f-bb79-931c66e6a1f2', 'Cuisine Américaine'),
('a5dc9e02-34b6-40ea-8b9b-7594a71cf12f', 'Cuisine Vegan');

INSERT INTO Plats (Id_Plat, Nom, Type_Plat, Pour_combien_de_personnes, Nationalite, Regime_Alimentaire, Ingrédients, Image, Date_de_fabrication, Date_de_Peremption, Prix_par_Portion) VALUES
('9c5e2a04-3e67-4f6c-a26b-ef891f3136f2', 'Quiche Lorraine', 'Plat Principal', 4, 'Française', 'Omnivore', 'Œufs, crème, lardons, pâte brisée', NULL, '2025-04-01 10:00:00', '2025-04-05 10:00:00', 8.50),
('41eb40d4-20b6-47c7-a963-e6bd8f3de278', 'Taboulé', 'Entree', 2, 'Libanaise', 'Végétarien', 'Semoule, menthe, tomates, citron', NULL, '2025-04-02 08:30:00', '2025-04-04 08:30:00', 5.00),
('cc3db9be-7987-4ff6-842c-5a9db1b20f2e', 'Tiramisu', 'Dessert', 6, 'Italienne', 'Végétarien', 'Mascarpone, œufs, café, biscuits', NULL, '2025-04-01 15:00:00', '2025-04-06 15:00:00', 4.50),
('d81c9d44-7e5f-4d82-bcf4-c0dc9e7a6c2c', 'Lasagnes végétariennes', 'Plat Principal', 4, 'Italienne', 'Végétarien', 'Pâtes, sauce tomate, légumes, fromage', NULL, '2025-04-01 12:00:00', '2025-04-05 12:00:00', 7.80),
('b5e6e8c3-7a1c-4a5c-b0e4-846cb06156d2', 'Poulet Yassa', 'Plat Principal', 3, 'Sénégalaise', 'Omnivore', 'Poulet, oignons, citron, moutarde', NULL, '2025-04-02 13:00:00', '2025-04-06 13:00:00', 9.20),
('cfa9dbe6-b9cc-40f6-bb14-01e8f4fd6d00', 'Baklava', 'Dessert', 5, 'Turque', 'Végétarien', 'Pâte filo, miel, noix', NULL, '2025-04-01 16:00:00', '2025-04-07 16:00:00', 4.00),
('e2d4501b-7f77-4956-991e-3c9f17f7e295', 'Falafels', 'Entree', 3, 'Moyen-Orient', 'Vegan', 'Pois chiches, coriandre, ail', NULL, '2025-04-03 10:00:00', '2025-04-06 10:00:00', 4.70),
('ab01fe0a-472d-41b6-89e4-4725b81d36f2', 'Mafé boeuf', 'Plat Principal', 4, 'Malienne', 'Omnivore', 'Boeuf, pâte d’arachide, légumes', NULL, '2025-04-02 11:30:00', '2025-04-06 11:30:00', 8.90),
('aceb3b9e-64f9-4a83-b4d0-d6b3d630b457', 'Cheesecake', 'Dessert', 6, 'Américaine', 'Végétarien', 'Fromage frais, sucre, biscuits', NULL, '2025-04-01 14:00:00', '2025-04-07 14:00:00', 4.80);
