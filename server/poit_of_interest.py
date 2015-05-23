__author__ = 'Florian'

class PointOfInterest(object):
    def __init__(self):
        self.id = None
        self.name = None
        self.description = None
        self.latitude = None
        self.longitude = None
        self.creator = None
        self.category = None

    def __str__(self):
        return str(self.id) + ": " + str(self.name)

    @staticmethod
    def get_test_item(test_id):
        poi = PointOfInterest()
        poi.id = test_id
        poi.name = "t_name"
        poi.category = "t_category"
        poi.creator = "t_creator"
        poi.description = "_description"
        poi.latitude = 12.34321
        poi.longitude = 43.21234
        return poi
