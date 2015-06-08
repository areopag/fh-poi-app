
import logging
from poit_of_interest import PointOfInterest
#import sys
from google.appengine.ext import ndb
#sys.modules['ndb'] = ndb

POINT_OF_INTEREST = 'point_of_interest_db'
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

        #entry = Entry(parent = ndb.Key(POINT_OF_INTEREST_KEY, POINT_OF_INTEREST))
        query = POI(name = poi.name, category = poi.category, creator = poi.creator, description = poi.description, latitude = poi.latitude, longitude = poi.longitude)
        entry_key = query.put()
        poi.id = entry_key.id()
        return poi

    def update_poi(self, poi):
        """
        Updates to PointOfInterest with the specified poi.id in the database.
        :param poi: [PointOfInterest] object to be updated
        :return: [PointOfInterest] same object
        """

        query = POI.get_by_id(5664683906301952)
        logger.info("\n shit %s", query)
        return query

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
        logger.info("\n Filter_dict: %s", filter_dict)
        query = POI.query(POI.name == 't_name', POI.category == 't_category')

        pois = []
        for poi in query:
            #name = ndb.Key(POINT_OF_INTEREST_KEY, POI.ID)
            #logger.info("\n shit %s", name)
            #category = ndb.Key(POINT_OF_INTEREST_KEY, poi.category )
            tmpPoi = PointOfInterest()
            tmpPoi.id = 134
            tmpPoi.name = poi.name
            tmpPoi.category = poi.category
            tmpPoi.creator = poi.creator
            tmpPoi.description = poi.description
            tmpPoi.latitude = poi.latitude
            tmpPoi.longitude = poi.longitude
            logger.info("\n poi_no %d %s",2, tmpPoi)
            pois.append(tmpPoi)





        return pois
