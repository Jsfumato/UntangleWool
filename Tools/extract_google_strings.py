import os
import codecs
import gspread
from oauth2client.service_account import ServiceAccountCredentials

from lxml import etree
from lxml.etree import CDATA
import xml.etree.ElementTree as ET

scope = ['https://spreadsheets.google.com/feeds']
credentials = ServiceAccountCredentials.from_json_keyfile_name('UntangleMeow-5593d5fd470a.json', scope)
gc = gspread.authorize(credentials)
doc = gc.open_by_key('162K9yTLBP_IRMtDwBS5jV3Ly6YGYzri71O-x-4qtyl8')
ws = doc.sheet1
print ws.title

LANGUAGES = []
root = etree.Element('Strings')
values = ws.get_all_values()

#parsing languages
for child in values[0][1:]:
    if child.strip() == 'English':
        child = ''
    elif child != '':
        child = child.strip().split()[0]
    LANGUAGES.append(child and '_%s' % child or '')

print len(LANGUAGES)

#parsing keys
for row in values[1:]:
    key = row[0]
    if not key:
        continue
        
    for i, col in enumerate(row[1:]):
        if i >= len(LANGUAGES):
            break
            
        value = (col or '').strip()
        if value or i == 0:
            child = etree.SubElement(root, 'String')
            child.attrib['Key'] = '%s%s' % (key, LANGUAGES[i])
            child.text = ('\n' in value or '\\n' in value or '<' in value) and CDATA(value) or value

#save file
for path in [os.path.join(os.path.dirname(__file__), '../UntangleMeow/Assets/Resources/Strings.xml')]:
    if not os.path.isfile(path): continue
    print path

    with codecs.open(path, "w", 'utf-8') as f:
        f.write(etree.tostring(root, encoding='UTF-8', pretty_print=True).decode('UTF-8'))