import React from 'react';
import {View} from 'react-native';
import {MonoText} from '../components/StyledText';
import {TabViewExample} from "../components/TabDemo";

export default class SettingsScreen extends React.Component {
    static navigationOptions = {
        title: 'Pau no cu do lulu',
    };

    render() {
        /* Go ahead and delete ExpoConfigView and replace it with your
         * content, we just wanted to give you a quick view of your config */
        return <View>
            <MonoText>Fuck yall</MonoText>
            <TabViewExample/>
        </View>;
    }
}
