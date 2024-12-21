#include <WiFi.h>
#include <HTTPClient.h>
#include <NetworkClientSecure.h>
#include <EEPROM.h>
#include "DHT.h"

#define DHTPIN 15
#define DHTTYPE DHT22
#define BUTTON_REGISTER_PIN 16
#define BUTTON_TURN_ON_OFF_PIN 17
#define WIFI_SSID "Wokwi-GUEST"
#define WIFI_PASSWORD ""
#define SENSOR_TYPE "2"

#define EEPROM_SIZE 5
#define DEVICE_ID_ADDR 0
#define WORKING_STATE_ADDR 4

int sensorId = -1;
bool isWorking = false;
int delaySecs = 0;

String serverUrl = "https://tdvjj1ts-7089.euw.devtunnels.ms/api";
String createSensorUrl = "/Sensor/create";
String createSensorLogUrl = "/SensorLog/create";

DHT dht(DHTPIN, DHTTYPE);

int readDeviceIdFromEEPROM();
bool readWorkingStateFromEEPROM();
void registerDevice();
void toggleWorkingState();
void sensorFunctionality();

void setup() {
  Serial.begin(115200);

  EEPROM.begin(EEPROM_SIZE);

  sensorId = readDeviceIdFromEEPROM();
  if (sensorId != -1) {
    Serial.print("Device ID loaded from EEPROM: ");
    Serial.println(sensorId);
  } else {
    Serial.println("No device ID found. Please press the button to register.");
  }

  isWorking = readWorkingStateFromEEPROM();

  dht.begin();

  pinMode(BUTTON_REGISTER_PIN, INPUT_PULLUP);
  pinMode(BUTTON_TURN_ON_OFF_PIN, INPUT_PULLUP);

  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);

  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi");
}

void loop() {
  if (digitalRead(BUTTON_REGISTER_PIN) == LOW) {
    Serial.println("Button Pressed! Registering device...");
    registerDevice();
    delaySecs++;
    delay(100);
  }
  if (digitalRead(BUTTON_TURN_ON_OFF_PIN) == LOW) {
    Serial.println("Button Pressed! Changing device state...");
    toggleWorkingState();
    delaySecs++;
    delay(100);
  }

  if (sensorId != -1 && isWorking && delaySecs >= 300) {
    delaySecs = 0;
    sensorFunctionality();
  }

  delaySecs++;
  delay(100);
}

void sensorFunctionality() {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin(serverUrl + createSensorLogUrl);

    float temperature = dht.readHumidity();

    if (isnan(temperature)) {
      Serial.println("Failed to read from DHT sensor!");
      return;
    }

    Serial.println(temperature);

    String jsonPayload = String("{\"sensorId\": ") +
                         String(sensorId) +
                         String(", \"value\": ") +
                         String(temperature) +
                         String("}");

    http.addHeader("Content-Type", "application/json");
    int httpResponseCode = http.POST(jsonPayload);

    if (httpResponseCode > 0) {
      Serial.println("Data sent successfully: " + String(httpResponseCode));
    } else {
      Serial.println("Error sending data: " + String(http.errorToString(httpResponseCode).c_str()));
    }

    http.end();
  } else {
    Serial.println("WiFi not connected");
  }
}

void registerDevice() {
  if (sensorId != -1) {
    Serial.println("Device is already registered.");
    Serial.print("Device ID: ");
    Serial.println(sensorId);
    return;
  }
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin(serverUrl + createSensorUrl);
    http.addHeader("Content-Type", "application/json");

    int httpResponseCode = http.POST(SENSOR_TYPE);

    if (httpResponseCode == 200) {
      String response = http.getString();
      sensorId = response.toInt();

      for (int i = 0; i < EEPROM_SIZE; i++) {
        EEPROM.write(DEVICE_ID_ADDR + i, (sensorId >> (i * 8)) & 0xFF);
      }
      EEPROM.commit();
      Serial.println("Device ID stored in EEPROM.");

      Serial.println("Device registered successfully!");
      Serial.print("Device ID: ");
      Serial.println(sensorId);
    } else {
      Serial.print("Failed to register device, HTTP code: ");
      Serial.println(httpResponseCode);
    }
    http.end();
  } else {
    Serial.println("WiFi not connected!");
  }
}

int readDeviceIdFromEEPROM() {
  int id = 0;
  for (int i = 0; i < EEPROM_SIZE; i++) {
    id |= (EEPROM.read(DEVICE_ID_ADDR + i) << (i * 8));
  }

  if (id == 0xFFFFFFFF || id == 0) {
    return -1;
  }
  return id;
}

void toggleWorkingState() {
  if (sensorId == -1) {
    Serial.println("Device is not registered.");
    Serial.println("Working state change canceled");
    return;
  }
  isWorking = !isWorking;
  EEPROM.write(WORKING_STATE_ADDR, isWorking ? 1 : 0);
  EEPROM.commit();
  Serial.println("Working state stored in EEPROM.");

  Serial.print("Device working state changed to: ");
  Serial.println(isWorking ? "On" : "Off");
}

bool readWorkingStateFromEEPROM() {
  return EEPROM.read(WORKING_STATE_ADDR) == 1;
}
