const tellers = require("../InputData/TellerData.json").Teller;
let customers = require("../InputData/CustomerData.json").Customer;

// sort tellers by multiplier (asc), customers by duration (desc)
let tellerSorted = tellers.sort((a, b) => a.Multiplier - b.Multiplier);
customers = customers.sort(
  (a, b) => parseInt(b.duration) - parseInt(a.duration)
);

// function for appointment
const appointment = (customer, teller) => {
  let duration;
  if (teller.SpecialtyType == customer.type) {
    duration = customer.duration * teller.Multiplier;
  } else duration = customer.duration;
  return { customer, teller, duration };
};

const appointmentList = [];

//iterate over customers, of all tellers matching type asign to first one (best multiplier)
//if none matches type, just go with the first one of the entire list
for (let customer of customers) {
  let teller =
    tellerSorted.filter((x) => x.SpecialtyType == customer.type).length != 0
      ? tellerSorted.filter((x) => x.SpecialtyType == customer.type)[0]
      : tellerSorted[0];

  const appt = appointment(customer, teller);
  appointmentList.push(appt);

  //remove teller from list when occupied by customer
  tellerSorted = tellerSorted.filter((x) => x.ID != teller.ID);
  //if all tellers taken, reset list and continue with next customer with fresh list
  if (tellerSorted.length == 0) {
    tellerSorted = tellers.sort((a, b) => a.Multiplier - b.Multiplier);
  }
}

//group tellers by ID and sum durations of all customer apponintments per teller

let tellerTimes = {};

for (let appointment of appointmentList) {
  if (appointment.teller.ID in tellerTimes) {
    tellerTimes[appointment.teller.ID] += parseInt(appointment.duration);
  } else {
    tellerTimes[appointment.teller.ID] = parseInt(appointment.duration);
  }
}

//find teller ID with the longest total duration (first instance)
//const longestTeller = Object.keys(tellerTimes).reduce((a, b) =>
//  tellerTimes[a] > tellerTimes[b] ? a : b
//);
//console.log(longestTeller);

//longest total duration of all tellers:
const maxTime = Object.values(tellerTimes).sort((a, b) => b - a)[0];

console.log(maxTime);
