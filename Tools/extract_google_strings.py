

json_key = json.load(open(os.path.join(os.path.dirname(__file__), 'woorido_untangle_meow.json'), 'r'))
scope = ['https://spreadsheets.google.com/feeds']
credentials = SignedJwtAssertionCredentials(json_key['client_email'], json_key['private_key'], scope)
gc = gspread.authorize(credentials)
doc = gc.open_by_key('120EQAeRI6NFaBk2QR09j0YPInu7bo3ypvQ3DqZOdCh8')
ws = doc.sheet1