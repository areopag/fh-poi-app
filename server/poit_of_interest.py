import json

class PointOfInterest(object):
    def __init__(self):
        self.id = None
        self.name = None
        self.description = None
        self.latitude = None
        self.longitude = None
        self.creator = None
        self.category = None

    def validate(self):
        val_errors = []
        strings = ["name", "description", "creator", "category"]
        longs = ["id"]
        floats = ["latitude", "longitude"]
        for s in strings:
            if not self.__dict__[s] is None and not isinstance(self.__dict__[s], str):
                self.__dict__[s] = str(self.__dict__[s])

        for i in longs:
            if not self.__dict__[i] is None and not isinstance(self.__dict__[i], long):
                val_errors.append("[" + i + "] needs to be an long number")

        for f in floats:
            if not self.__dict__[f] is None and not isinstance(self.__dict__[f], float):
                val_errors.append("[" + f + "] needs to be a floating number")

        return val_errors

    def __str__(self):
        return str(self.id) + ": " + str(self.name)

    @staticmethod
    def from_request(req):
        poi = PointOfInterest()
	try:
		poi.id = long(req.form["id"])
		poi.name = req.form["name"]
		poi.category = req.form["category"]
		poi.creator = req.form["creator"]
		poi.description = req.form["description"]
		poi.latitude = float(req.form["latitude"])
		poi.longitude = float(req.form["longitude"])
	except KeyError:
                pass
        return poi

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
