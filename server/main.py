from flask import Flask
import datastore
from poit_of_interest import PointOfInterest
from datastore import DataStore
app = Flask(__name__)
app.config['DEBUG'] = True

# Note: We don't need to call run() since our application is embedded within
# the App Engine WSGI application server.

@app.route('/')
def hello():
    """Return a friendly HTTP greeting."""
    return 'This is the Point of Interest REST Service by:<br />Maderbacher Florian<br />M&uuml;hlberger Robert<br />P&uuml;rer Robert'

@app.route('/poi/get_all_test')
def get_all_pois():
    store = DataStore()
    pois = store.get_all_poi()
    return str(pois)

@app.route('/poi/post_test')
def create_poi():
    store = DataStore()
    poi = PointOfInterest.get_test_item(None)
    poi = store.create_poi(poi)
    return str(poi)

@app.route('/poi/get_filter_test')
def get_pois_filter():
    store = DataStore()
    pois = store.get_poi_filtered([{"name": "t_name"},{"category": "t_category"}])
    return str(pois)

@app.route('/poi/id/get_test')
def get_poi():
    store = DataStore()
    poi = store.get_poi_by_id(1)
    return str(poi)

@app.route('/poi/id/post_test')
def update_poi():
    store = DataStore()
    poi = PointOfInterest.get_test_item(1)
    poi.name = "t_name_toUpdate"
    poi = store.update_poi(poi)
    return str(poi)

@app.route('/poi/id/delete_test')
def delete_poi():
    store = DataStore()
    store.remove_poi(1)
    return "Deleted"



@app.errorhandler(404)
def page_not_found(e):
    """Return a custom 404 error."""
    return 'Sorry, nothing at this URL.', 404
