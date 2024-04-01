import sys
from PyQt5.QtWidgets import QApplication, QMainWindow, QVBoxLayout, QWidget, QLineEdit, QPushButton, QLabel, QTableWidget, QTableWidgetItem, QMessageBox
from presenter import WeaponPresenter  # Supposons que vous avez déjà le présentateur

class WeaponView(QMainWindow):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("Weapon Details")
        self.setGeometry(100, 100, 600, 400)

        # Création du présentateur
        self.presenter = WeaponPresenter()

        # Création de la page de chargement d'une arme par son ID
        self.create_get_by_id_page()

        # Création de la page d'ajout d'une nouvelle arme
        self.create_add_weapon_page()

        # Connexion des signaux des boutons aux slots correspondants
        self.load_button.clicked.connect(self.load_weapon)
        self.load_all_button.clicked.connect(self.load_all_weapons)
        self.add_button.clicked.connect(self.show_add_weapon_page)
        self.save_button.clicked.connect(self.add_weapon)
        self.delete_button.clicked.connect(self.delete_weapon)  # Connexion du bouton Delete Weapon à la méthode delete_weapon

        # Connexion des signaux du présentateur aux méthodes de mise à jour de l'interface utilisateur
        self.presenter.weapon_loaded.connect(self.update_weapon_details)
        self.presenter.all_weapons_loaded.connect(self.display_all_weapons_table)
        self.presenter.error_occurred.connect(self.display_error)
        self.presenter.weapon_added.connect(self.display_weapon_added_message)  # Connexion du signal weapon_added à la méthode display_weapon_added_message
        self.presenter.weapon_deleted.connect(self.display_weapon_deleted_message)  # Connexion du signal weapon_deleted à la méthode display_weapon_deleted_message

    def create_get_by_id_page(self):
        # Création des widgets de la page de chargement d'une arme par son ID
        self.weapon_id_input = QLineEdit()
        self.load_button = QPushButton("Load Weapon")
        self.add_button = QPushButton("Add Weapon")
        self.load_all_button = QPushButton("Load All Weapons")
        self.delete_button = QPushButton("Delete Weapon")  # Ajout du bouton "Delete Weapon"
        self.weapon_details_label = QLabel("Weapon Details:")
        self.table = QTableWidget()

        # Layout vertical pour la page de chargement d'une arme par son ID
        layout = QVBoxLayout()
        layout.addWidget(self.weapon_id_input)
        layout.addWidget(self.load_button)
        layout.addWidget(self.add_button)
        layout.addWidget(self.load_all_button)
        layout.addWidget(self.delete_button)  # Ajout du bouton "Delete Weapon"
        layout.addWidget(self.weapon_details_label)
        layout.addWidget(self.table)

        # Widget contenant le layout vertical de la page de chargement d'une arme par son ID
        self.get_by_id_widget = QWidget()
        self.get_by_id_widget.setLayout(layout)

        # Affichage de la page de chargement d'une arme par son ID
        self.setCentralWidget(self.get_by_id_widget)

    def create_add_weapon_page(self):
        # Création des widgets de la page d'ajout d'une nouvelle arme
        self.name_input = QLineEdit()
        self.type_input = QLineEdit()
        self.manufacturer_input = QLineEdit()
        self.caliber_input = QLineEdit()
        self.magazine_capacity_input = QLineEdit()
        self.fire_rate_input = QLineEdit()
        self.ammo_count_input = QLineEdit()
        self.save_button = QPushButton("Add")

        # Layout vertical pour la page d'ajout d'une nouvelle arme
        layout = QVBoxLayout()
        layout.addWidget(QLabel("Name:"))
        layout.addWidget(self.name_input)
        layout.addWidget(QLabel("Type:"))
        layout.addWidget(self.type_input)
        layout.addWidget(QLabel("Manufacturer:"))
        layout.addWidget(self.manufacturer_input)
        layout.addWidget(QLabel("Caliber:"))
        layout.addWidget(self.caliber_input)
        layout.addWidget(QLabel("Magazine Capacity:"))
        layout.addWidget(self.magazine_capacity_input)
        layout.addWidget(QLabel("Fire Rate:"))
        layout.addWidget(self.fire_rate_input)
        layout.addWidget(QLabel("Ammo Count:"))
        layout.addWidget(self.ammo_count_input)
        layout.addWidget(self.save_button)

        # Widget contenant le layout vertical de la page d'ajout d'une nouvelle arme
        self.add_weapon_widget = QWidget()
        self.add_weapon_widget.setLayout(layout)
        self.add_weapon_widget.hide()  # Cacher cette page initialement

    def load_weapon(self):
        weapon_id = self.weapon_id_input.text()
        if not weapon_id:
            self.display_error("Please enter a valid weapon ID.")
            return
        self.presenter.load_weapon(int(weapon_id))

    def load_all_weapons(self):
        self.presenter.load_all_weapons()

    def show_add_weapon_page(self):
        self.get_by_id_widget.hide()  # Cacher la page de chargement par ID
        self.add_weapon_widget.show()  # Afficher la page d'ajout d'une arme

    def add_weapon(self):
        new_weapon_data = {
            "Name": self.name_input.text(),
            "Type": self.type_input.text(),
            "Manufacturer": self.manufacturer_input.text(),
            "Caliber": self.caliber_input.text(),
            "MagazineCapacity": int(self.magazine_capacity_input.text()),
            "FireRate": int(self.fire_rate_input.text()),
            "AmmoCount": int(self.ammo_count_input.text())
        }
        self.presenter.add_weapon(new_weapon_data)

    def delete_weapon(self):
        weapon_id = self.weapon_id_input.text()
        if not weapon_id:
            self.display_error("Please enter a valid weapon ID.")
            return
        confirmation = QMessageBox.question(self, 'Confirmation', f"Do you want to delete weapon with ID {weapon_id}?", QMessageBox.Yes | QMessageBox.No)
        if confirmation == QMessageBox.Yes:
            self.presenter.delete_weapon(int(weapon_id))

    def update_weapon_details(self, weapon):
        details_text = f"Name: {weapon.Name}\nType: {weapon.Type}\nManufacturer: {weapon.Manufacturer}\nCaliber: {weapon.Caliber}\nMagazine Capacity: {weapon.MagazineCapacity}\nFire Rate: {weapon.FireRate}\nAmmo Count: {weapon.AmmoCount}"
        self.weapon_details_label.setText(details_text)

    def display_all_weapons_table(self, weapons):
        self.table.setRowCount(len(weapons))
        self.table.setColumnCount(8)
        self.table.setHorizontalHeaderLabels(["ID", "Name", "Type", "Manufacturer", "Caliber", "Magazine Capacity", "Fire Rate", "Ammo Count"])
        for row, weapon in enumerate(weapons):
            self.table.setItem(row, 0, QTableWidgetItem(str(weapon.Id)))
            self.table.setItem(row, 1, QTableWidgetItem(weapon.Name))
            self.table.setItem(row, 2, QTableWidgetItem(weapon.Type))
            self.table.setItem(row, 3, QTableWidgetItem(weapon.Manufacturer))
            self.table.setItem(row, 4, QTableWidgetItem(weapon.Caliber))
            self.table.setItem(row, 5, QTableWidgetItem(str(weapon.MagazineCapacity)))
            self.table.setItem(row, 6, QTableWidgetItem(str(weapon.FireRate)))
            self.table.setItem(row, 7, QTableWidgetItem(str(weapon.AmmoCount)))

    def display_error(self, error_message):
        print(f"Error: {error_message}")

    def display_weapon_added_message(self, weapon_id):
        QMessageBox.information(self, 'Success', f"Weapon added successfully with ID: {weapon_id}")

    def display_weapon_deleted_message(self, weapon_id):
        QMessageBox.information(self, 'Success', f"Weapon deleted successfully with ID: {weapon_id}")

if __name__ == "__main__":
    app = QApplication(sys.argv)
    view = WeaponView()
    view.show()
    sys.exit(app.exec_())
