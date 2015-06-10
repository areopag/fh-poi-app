
import logging
import re

from poit_of_interest import PointOfInterest

from google.appengine.ext import ndb



POINT_OF_INTEREST_KEY = 'POI'

logger = logging.getLogger('test')
logger.setLevel(logging.DEBUG)

class POI(ndb.Model):
    name = ndb.StringProperty()
    category = ndb.StringProperty()
    creator = ndb.StringProperty()
    description = ndb.StringProperty()
    latitude = ndb.FloatProperty()
    longitude = ndb.FloatProperty()


class Entry(ndb.Model):
    poi = ndb.StructuredProperty(POI)


class DataStore(object):
    def __init__(self):
        # do some initialization if needed,
        self.conStr = ""


    """
    Inserts the PointOfInterest into the Database
    :param poi: [PointOfInterest] new object to be saved
    :return: [PointOfInterest] same object with updated id
    """
    def create_poi(self, poi):

        query = POI(name = poi.name, category = poi.category, creator = poi.creator, description = poi.description, latitude = poi.latitude, longitude = poi.longitude)
        entry_key = query.put()
        poi.id = entry_key.id()

        return poi




    """
    Updates to PointOfInterest with the specified poi.id in the database.
    :param poi: [PointOfInterest] object to be updated
    :return: [PointOfInterest] same object
    """
    def update_poi(self, poi):

        if poi == None:
            return None

        query = POI.get_by_id(poi.id)

        query.name = poi.name
        query.category = poi.category
        query.creator = poi.creator
        query.description = poi.description
        query.latitude = poi.latitude
        query.longitude = poi.longitude

        query.put()

        return poi


    """
    Delete the PointOfInterest with the given id
    :param poi_id: [int] id of poi to delete
    """
    def remove_poi(self, poi_id):

        if poi_id == None:
            return None

        query = POI.get_by_id(poi_id)
        if query == None:
            return None

        query.key.delete()

    """
    Read all PointOfInterest from the database.
    :return: [PointOfInterest[]] retrieved list of pois
    """
    def get_all_poi(self):


        query = POI.query()
        logger.info("\n *************** %s", query )
        if query == None:
            return None

        pois = []
        for poi in query:
            tmpPoi = self.getNewPoi(poi)
            pois.append(tmpPoi)
        return pois



    """
    Reads one single PointOfInterest with the given id.
    :param poi_id: id of poi to return
    :return: [PointOfInterest] found poi-object, None if id not found
    """
    def get_poi_by_id(self, poi_id):


        if poi_id == None:
            return None

        query = POI.get_by_id(poi_id)
        if query == None:
            return None

        tmpPoi = PointOfInterest()
        tmpPoi.id = poi_id
        tmpPoi.name = query.name
        tmpPoi.category = query.category
        tmpPoi.creator = query.creator
        tmpPoi.description = query.description
        tmpPoi.latitude = query.latitude
        tmpPoi.longitude = query.longitude

        return tmpPoi


    """
    Searches the database for all PointOfInterest-entries
    containing the values of the filter_dict combined with "and"-filters.
    :param filter_dict: [dict({string,object})] dictionary with
        the propertyname as key and
        the value to search for as value
    :return: [PointOfInterest[]] matching poi-objects
    """
    def get_poi_filtered(self, filter_dict):

        if filter_dict == None:
            return None

	key = filter_dict.keys()[0]
	value = filter_dict[key]

	tolerance = 0.01

	if key == "name":
		query = POI.query(POI.name == value)
	elif key == "latitude":
		query = POI.query(POI.latitude >= float(value) - tolerance, POI.latitude <= float(value) + tolerance)
	elif key == "longitude":
		query = POI.query(POI.longitude >= float(value) - tolerance, POI.longitude <= float(value) + tolerance)
	elif key == "creator":
		query = POI.query(POI.creator == value)
	elif key == "description":
		query = POI.query(POI.description == value)
	elif key == "category":
		query = POI.query(POI.category == value)

        if query == None:
            return None

        pois = []
        for poi in query:
            tmpPoi = self.getNewPoi(poi)
            pois.append(tmpPoi)

        return pois


    """
    Create a new point of interest and return it
    :param poi: poi from query
    :return: new poi
    """
    def getNewPoi(self, poi):
        tmpPoi = PointOfInterest()
        tmpPoi.id = self.getId(poi)
        tmpPoi.name = poi.name
        tmpPoi.category = poi.category
        tmpPoi.creator = poi.creator
        tmpPoi.description = poi.description
        tmpPoi.latitude = poi.latitude
        tmpPoi.longitude = poi.longitude

        return tmpPoi



    """
    Searches within the given poi all numbers.
    First detected number will be the id for the entry.
    :param poi: contains the wohle entry from the db
    :return: id
    """
    def getId(self, poi):
        id = re.findall('([0-9]+)', str(poi));
        return int(id[0]) # Cast first posistion to int
