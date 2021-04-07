import React from 'react'
import Alert from '@material-ui/lab/Alert';
import { Redirect } from 'react-router';

function AuthResultsAction(props) {
	const logoutAction = () => {
		fetch("api/account/logoff", {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
		});
	}

	const mainContent = () => {
		{
			if (props.msg === null) {
				props.error !== null
					? props.error.map(e => {
						return (
							<Alert variant="outlined" severity="warning">
								{e}
							</Alert>);
					})
					: logoutAction();
			}
			else return (
				<Redirect to="/" />
			);
		}
	}

	return (
		<div>
			{mainContent()}
		</div>
	)
}

export default AuthResultsAction
