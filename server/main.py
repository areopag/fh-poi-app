from flask import Flask
from flask import request
from flask import Response
from poit_of_interest import PointOfInterest
from datastore import DataStore
import json
app = Flask(__name__)
app.config['DEBUG'] = True

@app.route('/')
def hello():
    """Return a friendly HTTP greeting."""
    return 'This is the Point of Interest REST Service by:<br />Maderbacher Florian<br />M&uuml;hlberger Robert<br />P&uuml;rer Robert'

# Test-Methods--------------------------
@app.route('/test')
def test():
    return json.dumps(PointOfInterest())

@app.route('/test/poi/post')
def create_poi_test():
    store = DataStore()
    poi = PointOfInterest.get_test_item(None)
    poi = store.create_poi(poi)
    return to_json(poi)

@app.route('/test/poi/get_filter')
def get_pois_filter_test():
    store = DataStore()
    pois = store.get_poi_filtered([{"name": "t_name"}, {"category": "t_category"}])
    return to_json(pois)

@app.route('/test/poi/<int:id>/post')
def update_poi_test(id):
    store = DataStore()
    poi = PointOfInterest.get_test_item(id)
    poi.name = "t_name_toUpdate"
    poi = store.update_poi(poi)
    return to_json(poi)

@app.route('/test/poi/<int:id>/delete')
def delete_poi_test(id):
    store = DataStore()
    store.remove_poi(id)
    return "Deleted"

# --------------------------------------


@app.route('/poi', methods=["GET"])
def get_pois():
    store = DataStore()
    query_dic = dict()
    use_query = False
    for k in PointOfInterest().__dict__.keys():
        v = request.args.get(k, None)
        if v is not None:
            query_dic[k] = v
            use_query = True
    if use_query:
        pois = store.get_poi_filtered(query_dic)
    else:
        pois = store.get_all_poi()
    return to_json(pois)

@app.route('/poi', methods=["POST"])
def create_poi():
    store = DataStore()
    poi = PointOfInterest.from_request(request)
    poi.id = None
    valid = poi.validate()
    if len(valid) == 0:
        poi = store.create_poi(poi)
        return to_json(poi)
    else:
        return to_json(valid)

@app.route('/poi/<int:id>', methods=["GET"])
def get_poi(id):
    store = DataStore()
    poi = store.get_poi_by_id(id)
    return to_json(poi)

@app.route('/poi/<int:id>', methods=["POST"])
def update_poi(id):
    store = DataStore()
    poi = PointOfInterest.from_request(request)
    poi.id = id
    valid = poi.validate()
    if len(valid) == 0:
        poi = store.update_poi(poi)
        return to_json(poi)
    else:
        return to_json(valid)

@app.route('/poi/<int:id>', methods=["DELETE"])
def delete_poi(id):
    store = DataStore()
    store.remove_poi(id)
    return "Deleted"

def to_json(obj):
    o = to_serializable(obj)
    json_str = json.dumps(o, indent=4)
    return Response(json_str, mimetype="application/json")

def to_serializable(obj):
    if obj is None:
        return None
    if isinstance(obj, list):
        o = []
        for v in obj:
            o.append(to_serializable(v))
        return o
    elif isinstance(obj, PointOfInterest):
        return obj.__dict__
    else:
        return obj

@app.errorhandler(404)
def page_not_found(e):
    """Return a custom 404 error."""
    return 'Sorry, nothing at this URL.', 404
