Task
Your task is to write a program that reads lightning data as a stream from standard input (one lightning strike per line as a JSON object, and matches that data against a source of assets (also in JSON format) to produce an alert.

An example 'strike' coming off of the exchange looks like this:

{
    "flashType": 1,
    "strikeTime": 1386285909025,
    "latitude": 33.5524951,
    "longitude": -94.5822016,
    "peakAmps": 15815,
    "reserved": "000",
    "icHeight": 8940,
    "receivedTime": 1386285919187,
    "numberOfSensors": 17,
    "multiplicity": 1
}
Where:
flashType=(0='cloud to ground', 1='cloud to cloud', 9='heartbeat')
strikeTime=the number of milliseconds since January 1, 1970, 00:00:00 GMT
Note
A 'heartbeat' flashType is not a lightning strike. It is used internally by the software to maintain connection.
An example of an 'asset' is as follows:

  {
    "assetName":"Dante Street",
    "quadKey":"023112133033",
    "assetOwner":"6720"
  }
You might notice that the lightning strikes are in lat/long format, whereas the assets are listed in quadkey format.

There is a simple conversion process, outlined here that you can take advantage of. Feel free to use an open source library as well.

For this purpose, you can assume that all asset locations are at a zoom level of '12'.

For each strike received, you should simply print to the console the following message:

lightning alert for <assetOwner>:<assetName>
But substituting the proper assetOwner and assetName.

i.e.:

lightning alert for 6720:Dante Street
There's a catch though... Once we know lightning is in the area, we don't want to be alerted for it over & over again. Therefore, if you have already printed an alert for a lightning strike at a particular location, you should ignore any additional strikes that occur in that quadkey for that asset owner.

Implementation
Since code is read more often than it is written, we want to our projects well structured and the code easy to read. You should make sure your code lints against whatever standards are in common use by the language you choose (i.e. pep-8 for python)

Your program should also contain a README that contains information about the program and includes steps on how to run the program.

The files containing lightning strikes (as single JSON objects) and assets (as an array of JSON objects) can be found in this repo.

Feel free to use open source libraries where available..
