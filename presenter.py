import requests
from PyQt5.QtCore import QObject, pyqtSignal
from model import Weapon

class WeaponPresenter(QObject):
    weapon_loaded = pyqtSignal(Weapon)
    all_weapons_loaded = pyqtSignal(list)
    error_occurred = pyqtSignal(str)
    weapon_added = pyqtSignal(int)  # Signal pour indiquer que l'arme a été ajoutée avec succès
    weapon_deleted = pyqtSignal(int)  # Signal pour indiquer que l'arme a été supprimée avec succès

    def __init__(self):
        super().__init__()

    def load_weapon(self, weapon_id):
        try:
            # Faites une requête HTTP GET pour récupérer les détails de l'arme depuis l'API ASP.NET Core
            response = requests.get(f"http://localhost:5166/api/Weapon/{weapon_id}")
            if response.status_code == 200:
                weapon_data = response.json()
                weapon = Weapon(weapon_data['id'], weapon_data['name'], weapon_data['type'],
                                weapon_data['manufacturer'], weapon_data['caliber'],
                                weapon_data['magazineCapacity'], weapon_data['fireRate'],
                                weapon_data['ammoCount'])
                self.weapon_loaded.emit(weapon)
            else:
                self.error_occurred.emit(f"Failed to load weapon: {response.status_code}")
        except Exception as e:
            self.error_occurred.emit(f"An error occurred: {str(e)}")

    def load_all_weapons(self):
        try:
            # Faites une requête HTTP GET pour récupérer tous les weapons depuis l'API ASP.NET Core
            response = requests.get("http://localhost:5166/api/Weapon")
            if response.status_code == 200:
                weapons_data = response.json()
                weapons = [Weapon(weapon_data['id'], weapon_data['name'], weapon_data['type'],
                                weapon_data['manufacturer'], weapon_data['caliber'],
                                weapon_data['magazineCapacity'], weapon_data['fireRate'],
                                weapon_data['ammoCount']) for weapon_data in weapons_data]
                self.all_weapons_loaded.emit(weapons)
            else:
                self.error_occurred.emit(f"Failed to load weapons: {response.status_code}")
        except Exception as e:
            self.error_occurred.emit(f"An error occurred: {str(e)}")

    def add_weapon(self, weapon_data):
        try:
            response = requests.post("http://localhost:5166/api/Weapon", json=weapon_data)
            if response.status_code == 201:
                # Weapon successfully added
                new_weapon_id = response.json()['id']  # Get the ID of the newly added weapon
                self.weapon_added.emit(new_weapon_id)  # Emit the signal with the new weapon ID
            else:
                self.error_occurred.emit(f"Failed to add weapon: {response.status_code}")
        except Exception as e:
            self.error_occurred.emit(f"An error occurred: {str(e)}")

    def delete_weapon(self, weapon_id):
        try:
            response = requests.delete(f"http://localhost:5166/api/Weapon/{weapon_id}")
            if response.status_code == 200:
                # Weapon successfully deleted
                self.weapon_deleted.emit(weapon_id)  # Emit the signal with the deleted weapon ID
            else:
                self.error_occurred.emit(f"Failed to delete weapon: {response.status_code}")
        except Exception as e:
            self.error_occurred.emit(f"An error occurred: {str(e)}")
