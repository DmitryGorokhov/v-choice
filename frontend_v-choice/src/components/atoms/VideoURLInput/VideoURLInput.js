import { useState } from 'react'
import { TextField } from '@material-ui/core'

function VideoURLInput(props) {
	const [fullVideoUrl, setFullVideoUrl] = useState(props.token ? `https://youtu.be/${props.token}` : '');

	const handleOnChange = (event) => {
		const myregexp = /(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^"&?\/\s]{11})/;
		const value = event.target.value;
		const result = myregexp.test(value);

		if (result) {
			const arr = myregexp.exec(value);
			props.setToken(arr[1]);
		}
		else {
			props.setToken('');
		}

		setFullVideoUrl(value);
		props.setIsValid(result || !value);
	}

	return (
		<TextField
			margin="dense"
			id="video"
			label="Ссылка на видео"
			type="input"
			className={props.className}
			onChange={handleOnChange}
			value={fullVideoUrl}
			error={!props.isValid}
			fullWidth
		/>
	)
}

export default VideoURLInput