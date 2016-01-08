from flask import Flask,Response,json, request
from piazza_api import Piazza
from piazza_api.rpc import PiazzaRPC

import jsonpickle
app = Flask(__name__)

# Make the WSGI interface available at the top level so wfastcgi can get it.
wsgi_app = app.wsgi_app


@app.route('/')
def login():
    p = Piazza()
    p.user_login("nilavghosh@gmail.com", "Nbg2001@")
    cookies = jsonpickle.encode(p._rpc_api.cookies)
    return Response(cookies, mimetype="application/json")

@app.route('/post', methods = ['POST'])
def post():
    # Get the parsed contents of the form data
    json = request.json
    print(json)
    # Render template
    return jsonify(json)

@app.route('/GetClasses', methods = ['POST'])
def GetClasses():
    dat = str(request.data)
    piazzaSessionData = jsonpickle.decode(request.data.decode("utf-8"))
    p = Piazza()
    p._rpc_api = PiazzaRPC()
    p._rpc_api.cookies = piazzaSessionData
    classes = p.get_user_classes();
    #cnetclass = p.network("i9fr9tmxqa366k")
    #return Response(json.dumps(cnetclass.get_feed(10,0)), mimetype="application/json")
    return Response(json.dumps(classes), mimetype="application/json")


@app.route('/GetFeed', methods = ['POST'])
def GetFeed():
    dat = str(request.data)
    jsondata = jsonpickle.decode(request.data.decode("utf-8"))
    networkId = jsondata["ClassId"]
    piazzaSessionData = jsondata["Cookies"]
    limit = jsondata["Limit"]
    offset = jsondata["Offset"]
    p = Piazza()
    p._rpc_api = PiazzaRPC()
    p._rpc_api.cookies = piazzaSessionData
    cnetclass = p.network(networkId)
    return Response(json.dumps(cnetclass.get_feed(limit,offset)), mimetype="application/json")
    #return Response(json.dumps(classes), mimetype="application/json")

@app.route('/GetPost', methods = ['POST'])
def GetPost():
    dat = str(request.data)
    jsondata = jsonpickle.decode(request.data.decode("utf-8"))
    feedId = jsondata["FeedId"]
    networkId = jsondata["ClassId"] 
    piazzaSessionData = jsondata["Cookies"]
    p = Piazza()
    p._rpc_api = PiazzaRPC()
    p._rpc_api.cookies = piazzaSessionData
    cnetclass = p.network(networkId)
    return Response(json.dumps(cnetclass.get_post(feedId)), mimetype="application/json")
    #return Response(json.dumps(classes), mimetype="application/json")


@app.route('/GetPostDummy')
def GetPostDummy():
    p = Piazza()
    p.user_login("nilavghosh@gmail.com", "Nbg2001@")
    cnetclass = p.network("i9fr9tmxqa366k")
    return Response(json.dumps(cnetclass.get_post(41)), mimetype="application/json")
    #return Response(json.dumps(classes), mimetype="application/json")

if __name__ == '__main__':
    import os
    host = os.environ.get('SERVER_HOST', 'localhost')
    try:
        port = int(os.environ.get('SERVER_PORT', '5555'))
    except ValueError:
        port = 5555
    app.run(host, port)
