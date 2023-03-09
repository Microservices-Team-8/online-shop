import {useEffect, useState} from "react";

const UNKNOWN = "UNKNOWN"
const UNAVAILABLE = "UNAVAILABLE"

function App() {
  const [basketState, basketSetState] = useState(UNKNOWN);
  const [storeState, storeSetState] = useState(UNKNOWN);
  const [usersState, usersSetState] = useState(UNKNOWN);
  const [ordersState, ordersSetState] = useState(UNKNOWN);

  useEffect(() => {
    fetch('/api/basket')
        .then((response) => {
            return response.text();
        })
        .then((data) => {
            basketSetState(data);
        }).catch(() => {
      basketSetState(UNAVAILABLE)
    });
  }, []);

  useEffect(() => {
    fetch('/api/orders')
        .then((response) => {
            return response.text();
        })
        .then((data) => {
            ordersSetState(data);
        }).catch(() => {
      ordersSetState(UNAVAILABLE)
    });
  }, []);

  useEffect(() => {
    fetch('/api/store')
        .then((response) => {
            return response.text();
        })
        .then((data) => {
            storeSetState(data);
        }).catch(() => {
      storeSetState(UNAVAILABLE)
    });
  }, []);

  useEffect(() => {
    fetch('/api/users')
        .then((response) => {
            return response.text();
        })
        .then((data) => {
            usersSetState(data);
        }).catch(() => {
      usersSetState(UNAVAILABLE)
    });
  }, []);

  return (
    <div>
        basket-service status: { basketState }<br/>
        store-service status: { storeState }<br/>
        users-service status: { usersState }<br/>
        orders-service status: { ordersState }<br/>
    </div>
  );
}

export default App;
