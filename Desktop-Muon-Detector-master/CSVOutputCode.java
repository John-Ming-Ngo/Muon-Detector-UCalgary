	/**
	 * This function outputs the muon data into a CSV file.
	 * @Params none
	 * @Returns none
	 */
	public void outputCSV(Context context) {
		String fileName = "MuonEventData" + getCurrentTime().toString() + ".csv";

		File CSVout = new File(context.getFilesDir(), fileName);

		try {
			BufferedWriter textWriter = new BufferedWriter(new FileWriter(CSVout));
			textWriter.write("Titles of things here.");
			textWriter.newLine();

			for (MuonEvent event:eventData) {
				String data = event.toString();
				String[] dataParts = data.split("&");
				String[] rawDataParts = dataParts[3].split(",");
				String rawDataString = "";
				for (String part:rawDataParts) {
					rawDataString = rawDataString + part + "||";
				}
				dataParts[3] = rawDataString;

				String dataString = "";
				for (String part:dataParts) {
					dataString = dataString + part + ",";
				}
				textWriter.write(dataString);
				textWriter.newLine();
			}
			textWriter.close();
		}

		catch (IOException inputIssue) {
			inputIssue.printStackTrace();
		}

	}