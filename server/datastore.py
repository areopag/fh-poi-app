__author__ = 'Florian'
from poit_of_interest import PointOfInterest

class DataStore(object):
    def __init__(self):
        # do some initialization if needed,
        self.conStr = ""

    def create_poi(self, poi):
        """
        Inserts the PointOfInterest into the Database
        :param poi: [PointOfInterest] new object to be saved
        :return: [PointOfInterest] same object with updated id
        """
        return poi

    def update_poi(self, poi):
        """
        Updates to PointOfInterest with the specified poi.id in the database.
        :param poi: [PointOfInterest] object to be updated
        :return: [PointOfInterest] same object
        """
        return poi

    def remove_poi(self, poi_id):
        """
        Delete the PointOfInterest with the given id
        :param poi_id: [int] id of poi to delete
        """

    def get_all_poi(self):
        """
        Read all PointOfInterest from the database.
        :return: [PointOfInterest[]] retrieved list of pois
        """
        pois = []
        return pois

    def get_poi_by_id(self, poi_id):
        """
        Reads one single PointOfInterest with the given id.
        :param poi_id: id of poi to return
        :return: [PointOfInterest] found poi-object, None if id not found
        """
        poi = None
        return poi

    def get_poi_filtered(self, filter_dict):
        """
        Searches the database for all PointOfInterest-entries
        containing the values of the filter_dict combined with "and"-filters.
        :param filter_dict: [dict({string,object})] dictionary with
            the propertyname as key and
            the value to search for as value
        :return: [PointOfInterest[]] matching poi-objects
        """
        pois = []
        return pois
